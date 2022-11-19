using Api.Consts;
using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Services;
using Common.Extentions;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Api.Controllers
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
            //links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostContent), new
            //{
            //    postContentId = x.Id,
            //});
            //links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            //{
            //    userId = x.Id,
            //});
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(CreatePostRequest request)
        {
            if (!request.UserId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new Exception("not authorize");
                request.UserId = userId;
            }

            await _postService.CreatePost(request);
        }

        [HttpGet]
        public async Task<List<PostModel>> GetPosts(int skip = 0, int take = 10)
            => await _postService.GetPosts(skip, take);


        [HttpPost]
        [Authorize]
        public async Task AddCommentToPost(CreateCommentRequest request, Guid postId)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("You are not authorized");

            var model = new CreateCommentModel
            {
                UserId = userId,
                PostId = postId,
                Caption = request.Caption
            };

            await _postService.AddComment(model);
        }

        [HttpGet]
        public async Task<FileStreamResult> GetPostContent(Guid postContentId, bool download = false) //не робит
        {
            var attach = await _postService.GetPostImage(postContentId);
            var fs = new FileStream(attach.FilePath, FileMode.Open);
            if (download)
                return File(fs, attach.MimeType, attach.Name);
            else
                return File(fs, attach.MimeType);
        }

        [HttpGet]
        //public async Task<List<CommentModel>> GetPostComments(Guid postContentId)
        //    => await _postService.GetPostComments(postContentId);


        [HttpDelete]
        [Authorize]
        public async Task DeletePost(Guid id)
            => await _postService.DeletePost(id);

        [HttpGet]
        public async Task<PostModel> GetPost(Guid id)
            => await _postService.GetPost(id);
    }
}
