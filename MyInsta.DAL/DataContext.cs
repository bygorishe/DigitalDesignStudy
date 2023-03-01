using MyInsta.DAL.Entities.Attaches;
using MyInsta.DAL.Entities.Chats;
using MyInsta.DAL.Entities.Likes;
using MyInsta.DAL.Entities.Posts;
using MyInsta.DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace MyInsta.DAL
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(b => b.MigrationsAssembly("MyInsta.DAL"));

        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<UserSession> UserSessions => Set<UserSession>();
        public DbSet<Attach> Attaches => Set<Attach>();
        public DbSet<Avatar> Avatars => Set<Avatar>();
        public DbSet<PostImage> PostImages => Set<PostImage>();
        public DbSet<Like> Likes => Set<Like>();
        public DbSet<MessageLike> MessageLikes => Set<MessageLike>();
        public DbSet<PostLike> PostLikes => Set<PostLike>();
        public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
        public DbSet<Subscribtion> Subscribtions => Set<Subscribtion>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<Tag> Tags => Set<Tag>();
    }
}