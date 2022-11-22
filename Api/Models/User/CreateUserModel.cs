using System.ComponentModel.DataAnnotations;

namespace Api.Models.User
{
    public class CreateUserModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string? About { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare(nameof(Password))]
        public string RetryPassword { get; set; } = null!;
        [Required]
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; }
    }
}
