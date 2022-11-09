using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
            _postService.SetLinkGenerator(
            linkAvatarGenerator: x =>
            Url.Action(nameof(UserController.GetUserAvatar), "User", new
            {
               userId = x.Id,
              download = false
            }),
            linkContentGenerator: x => Url.Action(nameof(GetPostContent), new
            {
                postContentId = x.Id,
                download = false
            }));
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(CreatePostRequest request)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("You are not authorized");

            var model = new CreatePostModel
            {
                UserId = userId,
                Description = request.Description,
                Contents = request.Contents.Select(x =>
                new MetaWithPath(x, q => Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "attaches",
                    q.TempId.ToString()), userId)).ToList()
            };

            //model.Contents.ForEach(x =>
            //{
            //    var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), x.TempId.ToString()));
            //    if (tempFi.Exists)
            //    {
            //        var destFi = new FileInfo(x.FilePath);
            //        if (destFi.Directory != null && !destFi.Directory.Exists)
            //            destFi.Directory.Create();

            //        System.IO.File.Copy(tempFi.FullName, x.FilePath, true);
            //        tempFi.Delete();
            //    }

            //});

            await _postService.CreatePost(model, userId);
        }

        [HttpGet]
        public async Task<List<PostModel>> GetPosts(int skip = 0, int take = 10)
            => await _postService.GetPosts(skip, take);

        //[HttpPost]
        //public async Task AddImagesToPost(List<MetadataModel> model, Guid id)
        //    => await _postService.AddImagesToPost(id, model);

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
            var attach = await _postService.GetPostContent(postContentId);
            var fs = new FileStream(attach.FilePath, FileMode.Open);
            if (download)
                return File(fs, attach.MimeType, attach.Name);
            else
                return File(fs, attach.MimeType);
        }

        [HttpGet]
        public async Task<List<CommentModel>> GetPostComment(Guid postContentId)
        {
            return await _postService.GetPostComments(postContentId);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeletePost(Guid id)
            => await _postService.DeletePost(id);

        [HttpGet]
        public async Task<PostModel> GetPost(Guid id)
            => await _postService.GetPost(id);
    }
}
