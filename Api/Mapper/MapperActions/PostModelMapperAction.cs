//using Api.Consts;
//using Api.Models.Post;
//using AutoMapper;
//using Common.Extentions;
//using DAL.Entities;
//using System.Security.Claims;

//namespace Api.Mapper.MapperActions
//{
//    public class PostModelMapperAction : IMappingAction<Post, PostModel>
//    {
//        ClaimsPrincipal _claims;
//        public PostModelMapperAction(ClaimsPrincipal claims)
//        {
//            _claims = claims;
//        }
//        public void Process(Post source, PostModel destination, ResolutionContext context)
//            => destination.IsLiked = source.Likes.Any(x => x.UserId == _claims.GetClaimValue<Guid>(ClaimNames.Id));
//    }
//}
