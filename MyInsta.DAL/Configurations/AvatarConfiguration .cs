using MyInsta.DAL.Entities.Attaches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
    {
        public void Configure(EntityTypeBuilder<Avatar> builder)
            => builder.ToTable("Avatars");
    }
}
