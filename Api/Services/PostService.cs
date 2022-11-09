using Api.Configs;
using AutoMapper;
using DAL.Entities;
using DAL;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private Func<AttachModel, string?>? _linkContentGenerator;
        private Func<UserModel, string?>? _linkAvatarGenerator;
        public void SetLinkGenerator(Func<AttachModel, string?> linkContentGenerator, Func<UserModel, string?> linkAvatarGenerator)
        {
            _linkAvatarGenerator = linkAvatarGenerator;
            _linkContentGenerator = linkContentGenerator;
        }

        public PostService(IMapper mapper, DataContext context, UserService userService)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreatePost(CreatePostModel model, Guid userId)
        {
            var dbPost = _mapper.Map<Post>(model);
            await _context.Posts.AddAsync(dbPost);
            await _context.SaveChangesAsync();
        }

        private async Task<Post> GetPostById(Guid id)
        {
            var post = await _context.Posts
                .Include(x => x.Author)
                .Include(x => x.PostImages)
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                throw new Exception("Post not found");
            return post;
        }

        public async Task<PostModel> GetPost(Guid id)
        {
            var post = await GetPostById(id);
            var res = new PostModel
            {
                Author = new UserAvatarModel(_mapper.Map<UserModel>(post.Author), post.Author.Avatar == null ? null : _linkAvatarGenerator),
                Description = post.Description,
                Id = post.Id,
                Images = post.PostImages.Select(x =>
                new AttachWithLinkModel(_mapper.Map<AttachModel>(x), _linkContentGenerator)).ToList(),
                //Comments = post.Comments?.Select(x =>
                //new CommentModel(x)).ToList()
            };
            return res;
        }

        public async Task<List<PostModel>> GetPosts(int skip, int take)
        {
            var posts = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostImages)
                .Include(x => x.Comments)
                .AsNoTracking().Take(take).Skip(skip).ToListAsync();
            var res = posts.Select(post =>
                new PostModel
                {
                    Author = new UserAvatarModel(_mapper.Map<UserModel>(post.Author), post.Author.Avatar == null ? null : _linkAvatarGenerator),
                    Description = post.Description,
                    Id = post.Id,
                    Images = post.PostImages.Select(x =>
                    new AttachWithLinkModel(_mapper.Map<AttachModel>(x), _linkContentGenerator)).ToList(),
                    //Comments = post.Comments?.Select(x =>
                    //new CommentModel(x)).ToList()
                }).ToList();
            return res;
        }

        public async Task AddComment(CreateCommentModel model)
        {
            var dbComment = _mapper.Map<Comment>(model);
            await _context.Comments.AddAsync(dbComment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentModel>> GetPostComments(Guid postContentId)
        {
            var res = new List<CommentModel>();
            var post = await GetPostById(postContentId);
            foreach (var c in post.Comments)
            {
                var commentModel = new CommentModel(c);
                commentModel.User = _mapper.Map<UserModel>(c.Author);
                res.Add(commentModel);
            }
            return res;
        }

        public async Task<AttachModel> GetPostContent(Guid postContentId)
        {
            var res = await _context.PostImages.FirstOrDefaultAsync(x => x.Id == postContentId);
            return _mapper.Map<AttachModel>(res);
        }

        public async Task DeletePost(Guid postId)
        {
            var post = await GetPostById(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
