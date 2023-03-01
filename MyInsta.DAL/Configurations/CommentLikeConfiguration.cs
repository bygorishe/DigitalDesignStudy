using MyInsta.DAL.Entities.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
            => builder.ToTable("CommentLikes");
    }
}
