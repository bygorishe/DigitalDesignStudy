using MyInsta.DAL.Entities.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyInsta.DAL.Configurations
{
    public class MessageLikeConfiguration : IEntityTypeConfiguration<MessageLike>
    {
        public void Configure(EntityTypeBuilder<MessageLike> builder)
            => builder.ToTable("MessageLikes");
    }
}
