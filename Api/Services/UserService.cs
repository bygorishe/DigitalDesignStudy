using Api.Exceptions;
using Api.Models.Attach;
using Api.Models.User;
using AutoMapper;
using Common.Services;
using DAL;
using DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> CheckUserExist(string email)
            => await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());

        public async Task<bool> CheckUserExist(Guid id)
            => await _context.Users.AnyAsync(x => x.Id == id && !x.IsDeleted && x.IsVerified);

        public async Task CreateUser(CreateUserModel model)
        {
            var dbUser = _mapper.Map<User>(model);
            if (_context.Users.Any(x => x.Name == dbUser.Name && x.Id != dbUser.Id))
                throw new UserAlreadyExistException(dbUser.Name);
            var t = await _context.Users.AddAsync(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(x => x.Avatar)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null /*|| !user.IsVerified*/) //нет смысла отдельный эксепшн делать
                throw new UserNotFoundException();
            if (user.IsDeleted)
                throw new UserDeletedException();
            return user;
        }

        private async Task HardDeleteUser(Guid id)
        {
            var dbUser = await GetUserById(id);
            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var dbUser = await GetUserById(id);
            dbUser.IsDeleted = true;
            dbUser.DeleteDate = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
        }

        //public async Task RestoreUser(User dbUser)
        //{
        //    //var dbUser = await _context.Users
        //    //    .FirstOrDefaultAsync(x => x.Id == id);
        //    //if(dbUser == null)
        //    //    throw new UserNotFoundException();
        //    dbUser.IsDeleted = false;
        //    dbUser.DeleteDate = null;
        //    await _context.SaveChangesAsync();
        //}

        public async Task UpdateUserInformation(Guid id, UpdateUserModel model)
        {
            var dbUser = await GetUserById(id);
            if (model.Name != null)
            {
                if (_context.Users.Any(x => x.Name == model.Name))
                    throw new UserAlreadyExistException(model.Name);
                dbUser.Name = model.Name;
            }
            if (model.FullName != null) dbUser.FullName = model.FullName;
            if (model.About != null) dbUser.About = model.About;
            await _context.SaveChangesAsync();
        }

        public async Task ChangePassword(Guid id, string oldPass, string newPass, string retrynewPass)
        {
            var dbuser = await GetUserById(id);
            if (!HashServices.Verify(oldPass, dbuser.PasswordHash))
                throw new WrongPasswordException();
            var comparer = StringComparer.OrdinalIgnoreCase;
            if (comparer.Compare(newPass, retrynewPass) != 0)
                throw new NotEqualsPasswordsException();
            dbuser.PasswordHash = HashServices.GetHash(newPass);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAvatarModel>> GetUsers()
            => await _context.Users.AsNoTracking()
            .Include(x => x.Avatar)
            //.Include(x => x.Posts)
            //.Include(x => x.Subscribtions)
            //.Include(x => x.Followers)
            .Where(x => !x.IsDeleted/* && x.IsVerified*/)
            .Select(x => _mapper.Map<UserAvatarModel>(x))
            .ToListAsync();

        public async Task<UserAvatarModel> GetUser(Guid id)
            => _mapper.Map<User, UserAvatarModel>(await GetUserById(id));

        public async Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                var avatar = new DAL.Entities.Attaches.Avatar
                {
                    Author = user,
                    MimeType = meta.MimeType,
                    FilePath = filePath,
                    Name = meta.Name,
                    Size = meta.Size
                };
                user.Avatar = avatar;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AttachModel> GetUserAvatar(Guid userId)
        {
            var user = await GetUserById(userId);
            var atach = _mapper.Map<AttachModel>(user.Avatar);
            if (atach == null)
                throw new Exception("User dont have avatar");
            return atach;
        }
    }
}