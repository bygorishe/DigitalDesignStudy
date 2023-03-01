using MyInsta.DAL.Entities.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
    {
        public void Configure(EntityTypeBuilder<PostLike> builder)
            => builder.ToTable("PostLikes");
    }
}
