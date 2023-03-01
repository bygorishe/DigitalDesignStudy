using MyInsta.Api.Mapper.MapperActions;
using MyInsta.Api.Models.Attach;
using MyInsta.Api.Models.Chat;
using MyInsta.Api.Models.Comment;
using MyInsta.Api.Models.Like;
using MyInsta.Api.Models.Message;
using MyInsta.Api.Models.Post;
using MyInsta.Api.Models.Subscribtion;
using MyInsta.Api.Models.User;
using AutoMapper;
using MyInsta.DAL.Entities.Attaches;
using MyInsta.DAL.Entities.Chats;
using MyInsta.DAL.Entities.Likes;
using MyInsta.DAL.Entities.Posts;
using MyInsta.DAL.Entities.Users;
using MyInsta.Common.Services;

namespace MyInsta.Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashService.GetHash(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                .ForMember(d => d.RegistrateDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<User, UserModel>();
            CreateMap<User, UserAvatarModel>()
                .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
                .ForMember(d => d.SubscribtionsCount, m => m.MapFrom(s => s.Subscribtions!.Count))
                .ForMember(d => d.FollowersCount, m => m.MapFrom(s => s.Followers!.Count))
                .AfterMap<UserAvatarMapperAction>();

            CreateMap<CreatePostRequest, CreatePostModel>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<CreatePostModel, Post>()
                .ForMember(d => d.PostImages, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<Post, PostModel>()
                .ForMember(d => d.Contens, m => m.MapFrom(d => d.PostImages))
                .ForMember(d => d.LikesCount, m => m.MapFrom(d => d.Likes!.Count))
                .ForMember(d => d.CommentsCount, m => m.MapFrom(d => d.Comments!.Count));
                //.AfterMap<PostModelMapperAction>();

            CreateMap<Comment, CommentModel>()
                .ForMember(d => d.User, m => m.MapFrom(s => s.Author))
                .ForMember(d => d.LikeCount, m => m.MapFrom(s => s.Likes!.Count));
            CreateMap<CreateCommentModel, Comment>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));

            CreateMap<SubscribtionModel, Subscribtion>()
                .ForMember(d => d.SubscribeTime, m => m.MapFrom(s => DateTimeOffset.UtcNow))
                .ReverseMap();

            //CreateMap<CreateLikeModel, Like>()
            //    .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            //CreateMap<Like, LikeModel>()
            //    .ForMember(d => d.User, m => m.MapFrom(s => s.Author));

            CreateMap<CreateLikeModel, CommentLike>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<CommentLike, LikeModel>()
                .ForMember(d => d.User, m => m.MapFrom(s => s.Author));

            CreateMap<CreateChatModel, Chat>();
            //.AfterMap<ChatMapperAction>();
            CreateMap<Chat, ChatModel>()
                .ForMember(d => d.MessagesCount, m => m.MapFrom(s => s.Messages!.Count))
                .ForMember(d => d.MembersCount, m => m.MapFrom(s => s.Users!.Count));

            CreateMap<CreateMessageModel, Message>();
            CreateMap<Message, MessageModel>();

            CreateMap<Avatar, AttachModel>();
            CreateMap<PostImage, AttachModel>();
            CreateMap<PostImage, AttachExternalModel>().AfterMap<PostContentMapperAction>();
            CreateMap<MetadataModel, MetaLinkModel>();
            CreateMap<MetaLinkModel, PostImage>();
        }
    }
}
