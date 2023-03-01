using MyInsta.DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder
            //    .HasIndex(f => f.Email)
            //    .IsUnique();
            //builder
            //    .HasIndex(f => f.Name)
            //    .IsUnique();
            builder
                .HasMany(x => x.Followers)
                .WithOne(x => x.Follower)
                .HasForeignKey(x => x.FollowerId);
        }
    }
}
