using MyInsta.DAL.Entities.Attaches;
using MyInsta.DAL.Entities.Chats;
using MyInsta.DAL.Entities.Posts;

namespace MyInsta.DAL.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string? About { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
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
