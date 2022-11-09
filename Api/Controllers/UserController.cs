using Api.Models.Attach;
using Api.Models.User;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
            if (userService != null)
                _userService.SetLinkGenerator(x =>
            Url.Action(nameof(GetUserAvatar), new { userId = x.Id, download = false }));
        }

        [HttpPost]
        public async Task CreateUser(CreateUserModel model)
            => await _userService.CreateUser(model);

        [HttpGet]
        [Authorize]
        public async Task<List<UserModel>> GetUsers() 
            => await _userService.GetUsers();

        [HttpGet]
        [Authorize]
        public async Task<UserModel> GetCurrentUser()
        {
            var userIdString = User.Claims.FirstOrDefault(u => u.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
                return await _userService.GetUser(userId);
            else
                throw new Exception("You are not authorized");
        }

        [HttpDelete]
        public async Task DeleteUser(Guid id) 
            => await _userService.DeleteUser(id);

        [HttpPost]
        [Authorize]
        public async Task AddAvatarToUser(MetadataModel model)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
                if (!tempFi.Exists)
                    throw new Exception("file not found");
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
            else
                throw new Exception("you are not authorized");
        }

        [HttpGet]
        public async Task<FileResult> GetUserAvatar(Guid userId, bool download = false)
        {
            var attach = await _userService.GetUserAvatar(userId);
            var fs = new FileStream(attach.FilePath, FileMode.Open);
            if (download)
                return File(fs, attach.MimeType, attach.Name);
            else
                return File(fs, attach.MimeType);
        }
    }
}
