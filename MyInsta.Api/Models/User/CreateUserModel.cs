using System.ComponentModel.DataAnnotations;

namespace MyInsta.Api.Models.User
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string? About { get; set; }
        [Required]
        //[EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Введите пароль")]
        //[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string RetryPassword { get; set; } = null!;
        [Required(ErrorMessage = "Укажите дату рождения.")]
        public DateTimeOffset BirthDate { get; set; }
        //public DateTimeOffset RegistrateDate { get; set; }
    }
}
