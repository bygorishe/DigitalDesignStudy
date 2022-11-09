using Api.Configs;
using Api.Models.Attach;
using Api.Models.Token;
using Api.Models.User;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Services
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly AuthConfig _config;
        private Func<UserModel, string?>? _linkGenerator;
        public void SetLinkGenerator(Func<UserModel, string?> linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public UserService(IMapper mapper, DataContext context, IOptions<AuthConfig> config)
        {
            _mapper = mapper;
            _context = context;
            _config = config.Value;
        }

        public async Task CreateUser(CreateUserModel model)
        {
            var dbUser = _mapper.Map<User>(model);
            if (_context.Users.Any(x => x.Name == dbUser.Name && x.Id != dbUser.Id))
                throw new Exception($"User with loginName :{dbUser.Name} exist");
            var t = await _context.Users.AddAsync(dbUser);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var dbUser = await GetUserById(id);
            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateUserInformation() //новая модель или только свойства? createuser ???
        //{

        //}

        public async Task<List<UserModel>> GetUsers() 
            => await _context.Users.AsNoTracking().ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToListAsync();

        private async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new Exception("user not found");
            return user;
        }

        public async Task<UserModel> GetUser(Guid id) 
            => _mapper.Map<UserModel>(await GetUserById(id));

        private async Task<User> GetUserByCredention(string login, string pass)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == login.ToLower());
            if (user == null)
                throw new Exception("user not found");
            if (!HashHelper.Verify(pass, user.PasswordHash))
                throw new Exception("password is incorrect");
            return user;
        }

        private TokenModel GenerateTokens(UserSession session)
        {
            var dtNow = DateTime.Now;
            if (session.User == null)
                throw new Exception("magic");

            var jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                notBefore: dtNow,
                claims: new Claim[] {
            new Claim(ClaimsIdentity.DefaultNameClaimType, session.User.Name),
            new Claim("sessionId", session.Id.ToString()),
            new Claim("id", session.User.Id.ToString()),
            },
                expires: DateTime.Now.AddMinutes(_config.LifeTime),
                signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refresh = new JwtSecurityToken(
                notBefore: dtNow,
                claims: new Claim[] {
                new Claim("refreshToken", session.RefreshToken.ToString()),
                },
                expires: DateTime.Now.AddHours(_config.LifeTime),
                signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedRefresh = new JwtSecurityTokenHandler().WriteToken(refresh);

            return new TokenModel(encodedJwt, encodedRefresh);
        }

        public async Task<TokenModel> GetToken(string login, string password)
        {
            var user = await GetUserByCredention(login, password);
            var session = await _context.UserSessions.AddAsync(new UserSession
            {
                User = user,
                RefreshToken = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
            await _context.SaveChangesAsync();
            return GenerateTokens(session.Entity);
        }

        public async Task<UserSession> GetSessionById(Guid id)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(x => x.Id == id);
            if (session == null)
                throw new Exception("Session is not found");
            return session;
        }

        private async Task<UserSession> GetSessionByRefreshToken(Guid id)
        {
            var session = await _context.UserSessions.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshToken == id);
            if (session == null)
                throw new Exception("Session is not found");
            return session;
        }

        public async Task<TokenModel> GetTokenByRefreshToken(string refreshToken)
        {
            var validParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = _config.SymmetricSecurityKey()
            };
            var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtToken
                || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            if (principal.Claims.FirstOrDefault(x => x.Type == "refreshToken")?.Value is String refreshIdString
                && Guid.TryParse(refreshIdString, out var refreshId))
            {
                var session = await GetSessionByRefreshToken(refreshId);
                if (!session.IsActive)
                    throw new Exception("Session is not active");
                session.RefreshToken = Guid.NewGuid();
                await _context.SaveChangesAsync();
                return GenerateTokens(session);
            }
            else
                throw new SecurityTokenException("Invalid token");
        }

        public async Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                var avatar = new Avatar 
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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
