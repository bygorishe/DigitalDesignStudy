using Api.Consts;
using Api.Exceptions;
using Api.Models.Attach;
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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService, LinkGeneratorService links)
        {
            _userService = userService;
            links.LinkAvatarGenerator = x =>
            Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpGet]
        //[Authorize]
        public async Task<IEnumerable<UserAvatarModel>> GetUsers()
            => await _userService.GetUsers();

        [HttpGet]
        [Authorize]
        public async Task<UserAvatarModel> GetCurrentUser()
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new NotAuthorizedException();
            return await _userService.GetUser(userId);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteUser()
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new NotAuthorizedException();
            await _userService.DeleteUser(userId);
        }

        [HttpPost]
        [Authorize]
        public async Task UpdateUserInfo(UpdateUserModel model)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new NotAuthorizedException();
            await _userService.UpdateUserInformation(userId, model);
        }

        [HttpPost]
        [Authorize]
        public async Task ChangePassword(string oldPass, string newPass, string retryNewPass)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new NotAuthorizedException();
            await _userService.ChangePassword(userId, oldPass, newPass, retryNewPass);
        }

        [HttpPost]
        [Authorize]
        public async Task AddAvatarToUser(MetadataModel model)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            if (userId == default)
                throw new NotAuthorizedException();
            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
            if (!tempFi.Exists)
                throw new Exceptions.FileNotFoundException();
            else
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "attaches", model.TempId.ToString());
                var destFi = new FileInfo(path);
                if (destFi.Directory != null && !destFi.Directory.Exists)
                    destFi.Directory.Create();
                System.IO.File.Copy(tempFi.FullName, path, true);
                await _userService.AddAvatarToUser(userId, model, path);
            }
        }

        //[HttpGet]
        //public async Task<FileResult> GetUserAvatar(Guid userId, bool download = false)
        //{
        //    var attach = await _userService.GetUserAvatar(userId);
        //    var fs = new FileStream(attach.FilePath, FileMode.Open);
        //    if (download)
        //        return File(fs, attach.MimeType, attach.Name);
        //    else
        //        return File(fs, attach.MimeType);
        //}
    }
}
