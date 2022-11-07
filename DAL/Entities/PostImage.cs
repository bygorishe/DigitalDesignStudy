namespace DAL.Entities
{
    public class PostImage : Attach
    {
        public virtual Post Post { get; set; } = null!;
        // подписи к фото, отмечанные пользователи и итд.
        // (ради этого создан класс вместо аватар)
    }
}
