using Api.Models;
using AutoMapper;
using Common;
using DAL.Entities;

namespace Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Models.CreateUserModel, DAL.Entities.User>()
                .ForMember(u => u.Id, m => m.MapFrom(s => new Guid()))
                .ForMember(u => u.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(u => u.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                .ForMember(u => u.RegistrateDate, m => m.MapFrom(s => s.RegistrateDate.UtcDateTime))
                .ReverseMap();
            CreateMap<User, UserModel>();

            CreateMap<DAL.Entities.Post, Models.PostModel>()
                .ReverseMap();

            CreateMap<DAL.Entities.Comment, Models.CommentModel>()
                .ReverseMap();

            CreateMap<DAL.Entities.Avatar, Models.AttachModel>();
            CreateMap<DAL.Entities.PostImage, Models.AttachModel>();
        }
    }
}
