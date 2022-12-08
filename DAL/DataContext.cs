using DAL.Entities.Attaches;
using DAL.Entities.Chats;
using DAL.Entities.Likes;
using DAL.Entities.Posts;
using DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(f => f.Email)
                .IsUnique();
            modelBuilder
                .Entity<User>()
                .HasIndex(f => f.Name)
                .IsUnique();
            modelBuilder
                .Entity<User>()
                .HasMany(x => x.Followers)
                .WithOne(x => x.Follower)
                .HasForeignKey(x => x.FollowerId);

            modelBuilder.Entity<Avatar>().ToTable(nameof(Avatars));
            modelBuilder.Entity<PostImage>().ToTable(nameof(PostImages));
            modelBuilder.Entity<MessageLike>().ToTable(nameof(MessageLikes));
            modelBuilder.Entity<CommentLike>().ToTable(nameof(CommentLikes));
            modelBuilder.Entity<PostLike>().ToTable(nameof(PostLikes));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(b => b.MigrationsAssembly("Api"));

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