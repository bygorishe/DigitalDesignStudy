﻿namespace DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "empty";
        public string? FullName { get; set; } = "empty";
        public string? About { get; set; } = "empty";
        public string Email { get; set; } = "empty";
        public string PasswordHash { get; set; } = "empty";
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; }

        public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<UserSession>? Sessions { get; set; }
        //public virtual ICollection<Post>? Posts { get; set; }
    }
}
