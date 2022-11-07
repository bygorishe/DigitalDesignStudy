namespace Api.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string? FullName { get; set; }
        public string? About { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; }

        public UserModel(string name, string? fullName, string about, string email, DateTimeOffset birthDate, DateTimeOffset registrateDate)
        {
            Name = name;
            FullName = fullName;
            About = about;
            Email = email;
            BirthDate = birthDate;
            RegistrateDate = registrateDate;
        }
    }
}
