using MyInsta.Common.Consts;
using MyInsta.Common.Exceptions;
using MyInsta.Api.Models.Comment;
using MyInsta.Api.Models.Like;
using MyInsta.Api.Models.Post;
using MyInsta.Api.Services;
using MyInsta.Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyInsta.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService, LinkGeneratorService links)
        {
            _postService = postService;
            links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostImage), new
            {
                postContentId = x.Id,
            });
            links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(CreatePostRequest request)
        {
            if (!request.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                request.UserId = userId;
            }
            await _postService.CreatePost(request);
        }

        [HttpGet]
        public async Task<PostModel> GetPost(Guid id) //намерено без авторизации
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            //if (userId == default)
            //    throw new NotAuthorizedException();
            return await _postService.GetPost(id, userId);
        }

        [HttpGet]
        public async Task<List<PostModel>> GetPosts(int skip = 0, int take = 10)
        {
            var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
            //if (userId == default)
            //    throw new NotAuthorizedException();
            return await _postService.GetPosts(skip, take, userId);
        }

        [HttpPost]
        [Authorize]
        public async Task AddCommentToPost(CreateCommentModel model)
        {
            if (!model.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UserId = userId;
            }
            await _postService.AddComment(model);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeleteComment(Guid id)
            => await _postService.DeleteComment(id);

        [HttpPost]
        [Authorize]
        public async Task LikePost(CreateLikeModel model)
        {
            if (!model.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new NotAuthorizedException();
                model.UserId = userId;
            }
            await _postService.LikePost(model);
        }

        //[HttpDelete]
        //[Authorize]
        //public async Task DeleteLike(Guid id)
        //{
        //    var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        //    if (userId == default)
        //        throw new NotAuthorizedException();
        //    await _postService.UnlikePost(id, userId);
        //}

        //[HttpPost]
        //[Authorize]
        //public async Task LikeComment(CreateLikeModel model)
        //{
        //    if (!model.UserId.HasValue)
        //    {
        //        var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        //        if (userId == default)
        //            throw new NotAuthorizedException();
        //        model.UserId = userId;
        //    }
        //    await _postService.LikeComment(model);
        //}

        //[HttpDelete]
        //[Authorize]
        //public async Task DeleteCommentLike(Guid id)
        //{
        //    var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
        //    if (userId == default)
        //        throw new NotAuthorizedException();
        //    await _postService.UnlikeComment(id, userId);
        //}

        ////[HttpGet]
        ////[Authorize]
        ////public async Task<FileStreamResult> GetPostContent(Guid postContentId, bool download = false) //не робит
        ////{
        ////    var attach = await _postService.GetPostImage(postContentId);
        ////    var fs = new FileStream(attach.FilePath, FileMode.Open);
        ////    if (download)
        ////        return File(fs, attach.MimeType, attach.Name);
        ////    else
        ////        return File(fs, attach.MimeType);
        ////}

        //[HttpGet]
        //[Authorize]
        //public async Task<IEnumerable<CommentModel>> GetPostComments(Guid postId)
        //    => await _postService.GetPostComments(postId);

        //[HttpGet]
        //[Authorize]
        //public async Task<IEnumerable<LikeModel>> GetPostLikes(Guid postId)
        //    => await _postService.GetPostLikes(postId);

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<LikeModel>> GetCommentLikes(Guid postId)
            => await _postService.GetCommentLikes(postId);

        [HttpDelete]
        [Authorize]
        public async Task DeletePost(Guid id)
            => await _postService.DeletePost(id);
    }
}
