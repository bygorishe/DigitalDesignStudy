using AutoMapper;
using DAL;
using Microsoft.EntityFrameworkCore;
using Api.Models.Comment;
using Api.Models.Post;
using Api.Models.Attach;
using Api.Models.Like;
using Api.Exceptions;
using DAL.Entities.Posts;
using DAL.Entities.Likes;
using DAL.Entities.Users;
using Microsoft.Extensions.Hosting;

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
        #region Posts
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
            if (post == null || post.IsDeleted)
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
        {
            var posts = await _context.Posts
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.PostImages)
                .Include(x => x.Likes)
                .Include(x => x.Comments)
                .Where(x => !x.IsDeleted).AsNoTracking()
                .OrderByDescending(x => x.CreatedDate).Skip(skip).Take(take)
                //.Select(x => _mapper.Map<PostModel>(x))
                //.Select(x => x.IsLiked =)
                .ToListAsync();
            List<PostModel> postModels = new(posts.Count);
            foreach (var post in posts)
            {
                bool like = post.Likes!.Any(x => x.UserId == userId);
                var model = _mapper.Map<PostModel>(post);
                model.IsLiked = like;
                postModels.Add(model);
            }
            return postModels;
        }

        public async Task DeletePost(Guid postId)
        {
            var post = await GetPostById(postId);
            post.IsDeleted = true;
            post.DeleteDate = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<AttachModel> GetPostImage(Guid postContentId)
            => _mapper.Map<AttachModel>(await _context.PostImages.FirstOrDefaultAsync(x => x.Id == postContentId));
        #endregion

        #region Comments
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
            if (dbComment == null || dbComment.IsDeleted)
                throw new CommentNotFoundException();
            dbComment.IsDeleted = true;
            dbComment.DeleteDate = DateTimeOffset.Now;
            //_context.Comments.Remove(dbComment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetPostComments(Guid id, Guid userId)
        {
            var comments = await _context.Comments
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.PostId == id && !x.IsDeleted).AsNoTracking()
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            List<CommentModel> commentModels = new(comments.Count);
            foreach (var comment in comments)
            {
                bool like = comment.Likes!.Any(x => x.UserId == userId);
                var model = _mapper.Map<CommentModel>(comment);
                model.IsLiked = like;
                commentModels.Add(model);
            }
            return commentModels;
        }
        #endregion

        #region Likes
        public async Task LikePost(CreateLikeModel model)
        {
            var like = _context.PostLikes.FirstOrDefault(x => x.UserId == model.UserId && x.PostId == model.ObjectId);
            if (like == null)
                throw new LikeNotFoundException();
            if(!like.IsCanceled)
                throw new LikeAlreadyExistException();
            if (like.IsCanceled)
            {
                like.IsCanceled = false;
                like.CancelDate = null;
            }
            else
            {
                var dbLike = _mapper.Map<PostLike>(model);
                await _context.PostLikes.AddAsync(dbLike);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UnlikePost(Guid id, Guid userId)
        {
            var dbLike = _context.PostLikes.FirstOrDefault(x => x.PostId == id && x.UserId == userId && !x.IsCanceled);
            if (dbLike == null)
                throw new LikeNotFoundException();
            dbLike.IsCanceled = true;
            dbLike.CancelDate = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LikeModel>> GetPostLikes(Guid id)
            => await _context.PostLikes
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.PostId == id && !x.IsCanceled)
                .Select(x => _mapper.Map<LikeModel>(x))
                .ToListAsync();

        public async Task LikeComment(CreateLikeModel model)
        {
            var like = _context.CommentLikes
                .FirstOrDefault(x => x.UserId == model.UserId && x.CommentId == model.ObjectId);
            if (like == null)
                throw new LikeNotFoundException();
            if (!like.IsCanceled)
                throw new LikeAlreadyExistException();
            if (like.IsCanceled)
            {
                like.IsCanceled = false;
                like.CancelDate = null;
            }
            else
            {
                var dbLike = _mapper.Map<CommentLike>(model);
                await _context.CommentLikes.AddAsync(dbLike);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UnlikeComment(Guid id, Guid userId)
        {
            var dbLike = _context.CommentLikes
                .FirstOrDefault(x => x.CommentId == id && x.UserId == userId && !x.IsCanceled);
            if (dbLike == null)
                throw new LikeNotFoundException();
            _context.CommentLikes.Remove(dbLike);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LikeModel>> GetCommentLikes(Guid id)
            => await _context.CommentLikes
                .Include(x => x.Author).ThenInclude(x => x.Avatar)
                .Where(x => x.CommentId == id && !x.IsCanceled)
                .Select(x => _mapper.Map<LikeModel>(x))
                .ToListAsync();
        #endregion
    }
}
