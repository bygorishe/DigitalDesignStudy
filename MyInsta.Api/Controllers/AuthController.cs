using MyInsta.Common.Exceptions;
using MyInsta.Api.Models.Token;
using MyInsta.Api.Models.User;
using MyInsta.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInsta.DAL;

namespace MyInsta.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserService _userService;
        private readonly DataContext _context;

        public AuthController(AuthService authService, UserService userService, DataContext context)
        {
            _authService = authService;
            _userService = userService;
            _context = context;
        }

        [HttpPost]
        public async Task<TokenModel> Token(TokenRequestModel model)
            => await _authService.GetToken(model.Login, model.Pass);

        [HttpPost]
        public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
            => await _authService.GetTokenByRefreshToken(model.RefreshToken);

        [HttpPost]
        //[ApiExplorerSettings(GroupName = "Api")]
        public async Task RegisterUser(CreateUserModel model)
        {
            if (await _userService.CheckUserExist(model.Email))
                throw new UserAlreadyExistException("");
            await _userService.CreateUser(model);
        }

        [HttpPost]
        public async Task VerifiedUser(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            await _authService.VerifiedEmail(user);
        }
    }
}
