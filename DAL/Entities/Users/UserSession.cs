namespace DAL.Entities.Users
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } //вцеломтоинендо
        public Guid RefreshToken { get; set; }
        public DateTimeOffset Created { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual User User { get; set; } = null!;
    }
}
