using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Models.User;
using AutoMapper;
using Common;
using DAL.Entities;
using Microsoft.Extensions.Hosting;

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
                .ForMember(u => u.RegistrateDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<User, UserModel>();
            CreateMap<Post, PostModel>();
                //.ForMember(c => c.Images, m => m.MapFrom(s => s.PostImages?.Select(x =>
                //    new AttachWithLinkModel(x => x.Map<AttachModel>(x), _linkContentGenerator)).ToList()));
            CreateMap<CreatePostModel, Post>()
                .ForMember(u => u.Id, m => m.MapFrom(s => new Guid()));

            CreateMap<Comment, CommentModel>()
                .ReverseMap();

            CreateMap<Avatar, AttachModel>();
            CreateMap<PostImage, AttachModel>();

            CreateMap<MetadataModel, PostImage>();
            CreateMap<MetaWithPath, PostImage>();
            CreateMap<CreatePostModel, Post>()
                .ForMember(d => d.PostImages, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));

            CreateMap<CreateCommentModel, Comment>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow))
                .ReverseMap();
        }
    }
}
