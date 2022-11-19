using DAL.Entities;
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
            //modelBuilder
            //    .Entity<User>()
            //    .HasOne(b => b.Avatar)
            //.WithOne(i => i.Owner)
            //.HasForeignKey<Avatar>(b => b.OwnerId);

            modelBuilder.Entity<Avatar>().ToTable(nameof(Avatars));
            modelBuilder.Entity<PostImage>().ToTable(nameof(PostImages));
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
        public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
        public DbSet<Subscribtion> Subscribtions => Set<Subscribtion>();
    }
}