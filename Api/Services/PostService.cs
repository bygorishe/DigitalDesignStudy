using AutoMapper;
using DAL.Entities;
using DAL;
using Microsoft.EntityFrameworkCore;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Models.Attach;

namespace Api.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public PostService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreatePost(CreatePostRequest request)
        {
            var model = _mapper.Map<CreatePostModel>(request);
            model.Contents.ForEach(x =>
            {
                x.AuthorId = model.UserId;
                x.FilePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "attaches",
                    x.TempId.ToString());
                var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), x.TempId.ToString()));
                if (tempFi.Exists)
                {
                    var destFi = new FileInfo(x.FilePath);
                    if (destFi.Directory != null && !destFi.Directory.Exists)
                        destFi.Directory.Create();
                    File.Move(tempFi.FullName, x.FilePath, true);
                }
            });
            var dbPost = _mapper.Map<Post>(model);
            await _context.Posts.AddAsync(dbPost);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> GetPostById(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                throw new Exception("Post not found");
            return post;
        }

        public async Task<PostModel> GetPost(Guid id)
        {
            var post = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostImages)
                .Include(x => x.Comments)
                .Include(x => x.Likes)
                .AsNoTracking().Where(x => x.Id == id)
                .Select(x => _mapper.Map<PostModel>(x))
                .FirstOrDefaultAsync();
            if(post == null)
                throw new Exception("Post not found");
            return post;
        }

        public async Task<List<PostModel>> GetPosts(int skip, int take)
        {
            var posts = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostImages).AsNoTracking().OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take)
                .Include(x => x.Comments)
                .Include(x => x.Likes)
                .Select(x => _mapper.Map<PostModel>(x))
                .ToListAsync();
            return posts;
        }

        public async Task AddComment(CreateCommentModel model)
        {
            var dbComment = _mapper.Map<Comment>(model);
            await _context.Comments.AddAsync(dbComment);
            await _context.SaveChangesAsync();
        }

        //public async Task<List<CommentModel>> GetPostComments(Guid postContentId)
        //{
        //    var res = new List<CommentModel>();
        //    var post = await GetPostById(postContentId);
        //    foreach (var c in post.Comments)
        //    {
        //        var commentModel = new CommentModel(c);
        //        commentModel.User = _mapper.Map<UserModel>(c.Author);
        //        res.Add(commentModel);
        //    }
        //    return res;
        //}
        public async Task<AttachModel> GetPostImage(Guid postContentId)
        {
            var res = await _context.PostImages.FirstOrDefaultAsync(x => x.Id == postContentId);
            return _mapper.Map<AttachModel>(res);
        }

        public async Task DeletePost(Guid postId)
        {
            var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
