using MyInsta.Common.Consts;
using MyInsta.Common.Exceptions;
using MyInsta.Api.Models.Subscribtion;
using MyInsta.Api.Models.User;
using MyInsta.Api.Services;
using MyInsta.Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyInsta.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class SubscribtionController : ControllerBase
    {
        private readonly SubscribtionService _subscribtionService;
        private readonly UserService _userService;

        public SubscribtionController(SubscribtionService subscribtionService, UserService userService, LinkGeneratorService links)
        {
            _subscribtionService = subscribtionService;
            _userService = userService;
            links.LinkAvatarGenerator = x =>
            Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task Subscribe(SubscribtionModel model)
        {
            if (!model.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UserId = userId;
            }
            if (await _userService.CheckUserExist(model.FollowerId))
                throw new UserNotFoundException();
            await _subscribtionService.SubscribeToUser(model);
        }

        [HttpPost]
        [Authorize]
        public async Task Unsubscribe(SubscribtionModel model)
        {
            if (!model.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UserId = userId;
            }
            if (await _userService.CheckUserExist(model.FollowerId))
                throw new UserNotFoundException();
            await _subscribtionService.UnsubscribeFromUser(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserAvatarModel>> ShowSubsribes(Guid userId)
            => await _subscribtionService.GetSubscribtions(userId);

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserAvatarModel>> ShowFollowers(Guid userId)
            => await _subscribtionService.GetFollowers(userId);
    }
}
