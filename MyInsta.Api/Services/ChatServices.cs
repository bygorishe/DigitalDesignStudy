using AutoMapper;
using MyInsta.Api.Models.Chat;
using MyInsta.DAL.Entities.Chats;
using MyInsta.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using MyInsta.Api.Models.Message;
using MyInsta.Api.Models.Post;
using MyInsta.DAL;

namespace MyInsta.Api.Services
{
    public class ChatServices
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly UserService _userService;

        public ChatServices(IMapper mapper, DataContext context, UserService userService)
        {
            _mapper = mapper;
            _context = context;
            _userService = userService;
        }

        public async Task CreateChat(CreateChatModel model)
        {
            var dbChat = _mapper.Map<Chat>(model);
            foreach (var c in model.UsersId)
                dbChat.Users.Add(await _userService.GetUserById(c));
            await _context.Chats.AddAsync(dbChat);
            await _context.SaveChangesAsync();
        }

        private async Task<Chat> GetChatById(Guid id)
        {
            var chat = await _context.Chats
                .Include(x => x.Users).ThenInclude(x => x.Avatar)
                .Include(x => x.Messages)/*.ThenInclude(x => x.User).ThenInclude(x => x.Avatar)*/
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (chat == null)
                throw new ChatNotFoundException();
            return chat;
        }

        public async Task AddUserToChat(Guid userId, Guid chatId)
        {
            var dbChat = await GetChatById(chatId);
            dbChat.Users.Add(await _userService.GetUserById(userId));
            await _context.SaveChangesAsync();
        }

        public async Task AddMessegeToChat(CreateMessageModel model)
        {
            var dbChat = await GetChatById(model.ChatId);
            dbChat.Messages.Add(_mapper.Map<Message>(model));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageModel>> GetChatMessages(int skip, int take, Guid chatId)
            => await _context.Messages
                .Include(x => x.User).ThenInclude(x => x.Avatar)
                .Where(x => x.ChatId == chatId)
                .OrderBy(x => x.CreatedDate)
                .Skip(skip).Take(take)
                .Select(x => _mapper.Map<MessageModel>(x))
                .AsNoTracking()
                .ToListAsync();

        public async Task<ChatModel> GetChat(Guid chatId)
            => _mapper.Map<ChatModel>(await GetChatById(chatId));

        public async Task ReadMessage()
        {

        }

        public async Task ReadAllMessages()
        {

        }

        public async Task LikeMessage()
        {

        }

        public async Task UnlikeMessage()
        {

        }
    }
}
