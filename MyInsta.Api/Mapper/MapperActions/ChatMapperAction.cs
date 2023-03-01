//using Api.Models.Chat;
//using Api.Services;
//using AutoMapper;
//using DAL.Entities.ChatAssociations;

//namespace Api.Mapper.MapperActions
//{
//    public class ChatMapperAction : IMappingAction<CreateChatModel, Chat>
//    {
//        private UserService _userservice;
//        public ChatMapperAction(UserService userservice)
//        {
//            _userservice = userservice;
//        }
//        public async void Process(CreateChatModel source, Chat destination, ResolutionContext context)
//        {
//            foreach (var c in source.UsersId)
//            {
//                destination.Users.Add(await _userservice.GetUserById(c));
//            }
//        }
//    }
//}
