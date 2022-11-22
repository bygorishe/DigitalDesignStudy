using AutoMapper;
using DAL.Entities;
using DAL;
using Microsoft.EntityFrameworkCore;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Models.Attach;
using Api.Models.Like;
using Api.Exceptions;

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
            var post = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar) //нет смысла правильно считать кол-во постов, только ава нужна
                .Include(x => x.PostImages)
                .Include(x => x.Likes)
                .Include(x => x.Comments)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                throw new PostNotFoundException();
            return post;
        }

        public async Task<PostModel> GetPost(Guid id, Guid userId)
        {
            var post = await GetPostById(id);
            var res = _mapper.Map<PostModel>(post);
            res.IsLiked = post.Likes!.Any(x => x.UserId == userId);
            return res;
        }

        public async Task<List<PostModel>> GetPosts(int skip, int take, Guid userId)
            => await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostImages)
                .Include(x => x.Likes)
                .Include(x => x.Comments).AsNoTracking()
                .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take)
                .Select(x => _mapper.Map<PostModel>(x))
                //.Select(x => x.IsLiked =)
                .ToListAsync();

        public async Task AddComment(CreateCommentModel model)
        {
            var dbComment = _mapper.Map<Comment>(model);
            await _context.Comments.AddAsync(dbComment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Guid id)
        {
            var dbComment = _context.Comments
                .FirstOrDefault(x => x.Id == id);
            if (dbComment == null)
                throw new CommentNotFoundException();
            _context.Comments.Remove(dbComment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetPostComments(Guid id)
            => await _context.Comments
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.PostId == id)
                .Select(x => _mapper.Map<CommentModel>(x))
                .ToListAsync();

        public async Task LikePost(CreateLikeModel model)
        {
            if (await _context.Likes.AnyAsync(x => x.UserId == model.UserId && x.PostId == model.ObjectId))
                throw new LikeAlreadyExistException();
            var dbLike = _mapper.Map<Like>(model);
            await _context.Likes.AddAsync(dbLike);
            await _context.SaveChangesAsync();
        }

        public async Task UnlikePost(Guid id, Guid userId)
        {
            var dbLike = _context.Likes
                .FirstOrDefault(x => x.PostId == id && x.UserId == userId);
            if (dbLike == null)
                throw new LikeNotFoundException();
            _context.Likes.Remove(dbLike);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LikeModel>> GetPostLikes(Guid id)
            => await _context.Likes
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.PostId == id)
                .Select(x => _mapper.Map<LikeModel>(x))
                .ToListAsync();

        public async Task LikeComment(CreateLikeModel model)
        {
            if(await _context.CommentLikes.AnyAsync(x => x.UserId == model.UserId && x.CommentId == model.ObjectId))
                throw new LikeAlreadyExistException();
            var dbLike = _mapper.Map<CommentLike>(model);
            await _context.CommentLikes.AddAsync(dbLike);
            await _context.SaveChangesAsync();
        }

        public async Task UnlikeComment(Guid id, Guid userId)
        {
            var dbLike = _context.CommentLikes
                .FirstOrDefault(x => x.CommentId == id && x.UserId == userId);
            if (dbLike == null)
                throw new LikeNotFoundException();
            _context.CommentLikes.Remove(dbLike);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LikeModel>> GetCommentLikes(Guid id)
            => await _context.CommentLikes
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.CommentId == id)
                .Select(x => _mapper.Map<LikeModel>(x))
                .ToListAsync();

        public async Task<AttachModel> GetPostImage(Guid postContentId)
        {
            var res = await _context.PostImages.FirstOrDefaultAsync(x => x.Id == postContentId);
            return _mapper.Map<AttachModel>(res);
        }

        public async Task DeletePost(Guid postId)
        {
            var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
            if (post == null)
                throw new PostNotFoundException();
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
