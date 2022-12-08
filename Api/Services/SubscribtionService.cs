using Api.Exceptions;
using Api.Models.Subscribtion;
using Api.Models.User;
using AutoMapper;
using DAL;
using DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class SubscribtionService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public SubscribtionService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task SubscribeToUser(SubscribtionModel model)
        {
            var sub = _context.Subscribtions.FirstOrDefault(x => x.UserId == model.UserId && x.FollowerId == model.FollowerId);
            if (sub == null)
                throw new SubscridtionNotFoundException();
            if (!sub.IsCanceled)
                throw new SubscribtionAlreadyExistException();
            if (sub.IsCanceled)
            {
                sub.IsCanceled = false;
                sub.CancelTime = null;
            }
            else
            {
                var dbSub = _mapper.Map<Subscribtion>(model);
                var t = await _context.Subscribtions.AddAsync(dbSub);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeFromUser(SubscribtionModel model)
        {
            var dbSub = _context.Subscribtions
                .FirstOrDefault(x => x.UserId == model.UserId && x.FollowerId == model.FollowerId && !x.IsCanceled);
            if(dbSub == null)
                throw new SubscridtionNotFoundException();
            _context.Subscribtions.Remove(dbSub);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAvatarModel>> GetSubscribtions(Guid userId)
        => await _context.Subscribtions
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .OrderByDescending(x => x.SubscribeTime)
            .Include(x => x.Follower).ThenInclude(x => x.Avatar) //некрасиво, надо подумать
            .Include(x => x.Follower).ThenInclude(x => x.Posts)
            .Include(x => x.Follower).ThenInclude(x => x.Followers)
            .Include(x => x.Follower).ThenInclude(x => x.Subscribtions)
            .Where(x => !x.IsCanceled)
            .Select(x => _mapper.Map<UserAvatarModel>(x.Follower))
            .ToListAsync();

        public async Task<IEnumerable<UserAvatarModel>> GetFollowers(Guid userId)
        => await _context.Subscribtions
            .Where(x => x.FollowerId == userId)
            .AsNoTracking()
            .OrderByDescending(x => x.SubscribeTime)
            .Include(x => x.User).ThenInclude(x => x.Avatar)
            .Include(x => x.User).ThenInclude(x => x.Posts)
            .Include(x => x.User).ThenInclude(x => x.Followers)
            .Include(x => x.User).ThenInclude(x => x.Subscribtions)
            .Where(x => !x.IsCanceled)
            .Select(x => _mapper.Map<UserAvatarModel>(x.User))
            .ToListAsync();
    }
}

