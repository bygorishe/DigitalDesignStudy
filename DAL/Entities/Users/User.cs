using DAL.Entities.Attaches;
using DAL.Entities.Chats;
using DAL.Entities.Posts;

namespace DAL.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "empty";
        public string? FullName { get; set; } = "empty";
        public string? About { get; set; } = "empty";
        public string Email { get; set; } = "empty";
        public string PasswordHash { get; set; } = "empty";
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }

        public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<UserSession>? Sessions { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Subscribtion>? Subscribtions { get; set; }
        public virtual ICollection<Subscribtion>? Followers { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
    }
}
