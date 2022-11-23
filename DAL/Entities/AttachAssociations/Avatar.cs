using DAL.Entities.UserAssociations;

namespace DAL.Entities.AttachAssociations
{
    public class Avatar : Attach
    {
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; } = null!;
    }
}
