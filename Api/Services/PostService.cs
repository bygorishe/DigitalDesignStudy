using Api.Configs;
using Api.Models;
using AutoMapper;
using DAL.Entities;
using DAL;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Api.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserService _userService;

        public PostService(IMapper mapper, DataContext context, UserService userService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }

        public async Task CreatePost(CreatePostModel model, Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var dbPost = new Post
                {
                    UserId = userId,
                    Description = model.Description,
                    CreatedDate = model.CreatedDate,
                    Author = user,
                    PostImages = new List<PostImage>(),
                    Comments = new List<Comment>()
            };
                await _context.Posts.AddAsync(dbPost);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<Post> GetPostById(Guid id)
        {
            var post = await _context.Posts.Include(x => x.Author).Include(x => x.PostImages)
                .Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
            if (post == null)
                throw new Exception("post not found");
            return post;
        }

        public async Task AddImagesToPost(Guid postId, List<MetadataModel> meta)
        {
            var post = await GetPostById(postId);
            if (post != null)
            {
                foreach (var metadata in meta)
                {
                    var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), metadata.TempId.ToString()));
                    if (!tempFi.Exists)
                        throw new Exception("file not found");
                    else
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "attaches", metadata.TempId.ToString());
                        var destFi = new FileInfo(path);
                        if (destFi.Directory != null && !destFi.Directory.Exists)
                            destFi.Directory.Create();
                        File.Copy(tempFi.FullName, path, true);

                        var postImage = new PostImage
                        {
                            Author = post.Author,
                            MimeType = metadata.MimeType,
                            FilePath = path,
                            Name = metadata.Name,
                            Size = metadata.Size,
                            Post = post
                        };

                        post.PostImages.Add(postImage);
                    }
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddCommentToPost(CreateCommentModel comment, Guid postId, Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var post = await GetPostById(postId);
            if (user != null && post != null)
                post.Comments.Add(new Comment
                {
                    Author = user,
                    Caption = comment.Caption,
                    CreatedDate = comment.CreatedDate.UtcDateTime,
                    Post = post,
                    PostId = postId,
                    UserId = userId
                });
            await _context.SaveChangesAsync();
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
