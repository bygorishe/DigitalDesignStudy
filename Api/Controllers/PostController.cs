using Api.Models;
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
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(CreatePostModel model)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("You are not authorized");
            await _postService.CreatePost(model, userId);
        }

        [HttpPost]
        public async Task AddImagesToPost(List<MetadataModel> model, Guid id)
            => await _postService.AddImagesToPost(id, model);

        [HttpPost]
        [Authorize]
        public async Task AddCommentToPost(CreateCommentModel model, Guid id)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (!Guid.TryParse(userIdString, out var userId))
                throw new Exception("You are not authorized");
            await _postService.AddCommentToPost(model, id, userId);
        }

        [HttpDelete]
        [Authorize]
        public async Task DeletePost(Guid id)
            => await _postService.DeletePost(id);
    }
}
