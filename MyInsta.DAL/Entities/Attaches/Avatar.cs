using MyInsta.DAL.Entities.Users;

namespace MyInsta.DAL.Entities.Attaches
{
    public class Avatar : Attach
    {
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; } = null!;
    }
}
