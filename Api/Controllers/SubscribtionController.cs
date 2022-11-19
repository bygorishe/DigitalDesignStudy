using Api.Consts;
using Api.Models.Subscribtion;
using Api.Models.User;
using Api.Services;
using Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class SubscribtionController : ControllerBase
    {
        private readonly SubscribtionService _subscribtionService;

        public SubscribtionController(SubscribtionService subscribtionService, LinkGeneratorService links)
        {
            _subscribtionService = subscribtionService;
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
                    throw new Exception("You are not authorized");
                model.UserId = userId;
            }
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
                    throw new Exception("You are not authorized");
                model.UserId = userId;
            }
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
