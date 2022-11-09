using Api.Models.Attach;
using Api.Models.Post;
using Api.Models.User;
using System;
using System.Xml.Linq;

namespace Api.Models.Comment
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public string Caption { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }

        public virtual UserModel User { get; set; } = null!;

        public CommentModel(DAL.Entities.Comment comment)
        {
            Id = comment.Id;
            Caption = comment.Caption;
            CreatedDate = comment.CreatedDate;
        }
    }
}
