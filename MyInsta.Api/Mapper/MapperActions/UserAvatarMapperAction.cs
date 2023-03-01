using MyInsta.Api.Models.User;
using MyInsta.Api.Services;
using AutoMapper;
using MyInsta.DAL.Entities.Users;

namespace MyInsta.Api.Mapper.MapperActions
{
    public class UserAvatarMapperAction : IMappingAction<User, UserAvatarModel>
    {
        private LinkGeneratorService _links;
        public UserAvatarMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(User source, UserAvatarModel destination, ResolutionContext context) =>
            _links.FixAvatar(source, destination);

    }
}
