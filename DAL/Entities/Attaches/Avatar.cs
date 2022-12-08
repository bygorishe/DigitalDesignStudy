using DAL.Entities.Users;

namespace DAL.Entities.Attaches
{
    public class Avatar : Attach
    {
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; } = null!;
    }
}
