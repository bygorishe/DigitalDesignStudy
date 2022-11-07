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

        public async Task CreatePost(PostModel model, Guid userId)
        {
            var dbUser = await _userService.GetUser(userId);


            var dbPost = _mapper.Map<Post>(model);
            dbPost.Author = _mapper.Map<User>(dbUser);
            await _context.Posts.AddAsync(dbPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost()
        {

        }
    }
}
