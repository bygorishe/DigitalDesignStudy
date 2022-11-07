using System.ComponentModel.DataAnnotations;

namespace Api.Models
{

    public class CreateUserModel
    {
        [Required]
        public string Name { get; set; }
        public string? FullName { get; set; }
        public string? About { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string RetryPassword { get; set; }
        [Required]
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; } = DateTimeOffset.Now.UtcDateTime;

        public CreateUserModel(string name, string email, string password, string retryPassword, DateTimeOffset birthDate)
        {
            Name = name;
            Email = email;
            Password = password;
            RetryPassword = retryPassword;
            BirthDate = birthDate;
        }
    }
}
