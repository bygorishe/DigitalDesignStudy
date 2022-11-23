using Api.Consts;
using Api.Exceptions;
using Api.Models.Chat;
using Api.Models.Message;
using Api.Models.Post;
using Api.Services;
using Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class ChatController : ControllerBase
    {
        private readonly ChatServices _chatServices;
        private readonly UserService _userService;

        public ChatController(ChatServices chatServices, UserService userService, LinkGeneratorService links)
        {
            _chatServices = chatServices;
            _userService = userService;
            links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostImage), new
            {
                postContentId = x.Id,
            });
            links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpPost]
        //[Authorize]
        public async Task CreateChat(CreateChatModel model)
        {
            if (model.UsersId.Count == 0)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UsersId.Add(userId);
            }
            await _chatServices.CreateChat(model);
        }

        [HttpPost]
        //[Authorize]
        public async Task AddUser(Guid userId, Guid chatId)
            => await _chatServices.AddUserToChat(userId, chatId);

        [HttpPost]
        [Authorize]
        public async Task WriteMessage(CreateMessageModel model)
        {
            if (!model.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UserId = userId;
            }
            await _chatServices.AddMessegeToChat(model);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IEnumerable<MessageModel>> ShowMessages(Guid id, int skip = 0, int take = 10)
            => await _chatServices.GetChatMessages(skip, take, id);

        [HttpGet]
        //[Authorize]
        public async Task<ChatModel> AboutChat(Guid id)
            => await _chatServices.GetChat(id);
    }
}
