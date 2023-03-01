using MyInsta.DAL.Entities.Attaches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class PostImageConfiguration : IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
            => builder.ToTable("PostImages");
    }
}
