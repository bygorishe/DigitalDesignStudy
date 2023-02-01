using Api.Mapper.MapperActions;
using Api.Models.Attach;
using Api.Models.Chat;
using Api.Models.Comment;
using Api.Models.Like;
using Api.Models.Message;
using Api.Models.Post;
using Api.Models.Subscribtion;
using Api.Models.User;
using AutoMapper;
using Common;
using DAL.Entities.Attaches;
using DAL.Entities.Chats;
using DAL.Entities.Likes;
using DAL.Entities.Posts;
using DAL.Entities.Users;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, User>()
                //.ForMember(u => u.Id, m => m.MapFrom(s => new Guid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashServices.GetHash(s.Password)))
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
                .ForMember(d => d.LikeCount, m => m.MapFrom(s => s.Likes.Count));
            CreateMap<CreateCommentModel, Comment>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));

            CreateMap<SubscribtionModel, Subscribtion>()
                .ForMember(d => d.SubscribeTime, m => m.MapFrom(s => DateTimeOffset.UtcNow))
                .ReverseMap();

            CreateMap<CreateLikeModel, Like>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            //    .ForMember(d => d.PostId, m => m.MapFrom(s => s.ObjectId));
            CreateMap<Like, LikeModel>()
                .ForMember(d => d.User, m => m.MapFrom(s => s.Author));

            CreateMap<CreateLikeModel, CommentLike>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
             //   .ForMember(d => d.CommentId, m => m.MapFrom(s => s.ObjectId));
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
