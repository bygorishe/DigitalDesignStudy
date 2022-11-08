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
            CreateMap<CreateUserModel, User>()
                .ForMember(u => u.Id, m => m.MapFrom(s => new Guid()))
                .ForMember(u => u.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(u => u.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                .ForMember(u => u.RegistrateDate, m => m.MapFrom(s => s.RegistrateDate.UtcDateTime))
                .ReverseMap();
            CreateMap<User, UserModel>();

            CreateMap<Post, PostModel>()
                .ReverseMap();
            CreateMap<CreatePostModel, Post>()
                .ForMember(u => u.Id, m => m.MapFrom(s => new Guid()))
                .ReverseMap();

            CreateMap<Comment, Models.CommentModel>()
                .ReverseMap();

            CreateMap<Avatar, AttachModel>();
            CreateMap<PostImage, AttachModel>();
        }
    }
}
