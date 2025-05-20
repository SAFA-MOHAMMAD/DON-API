using DON.Models;
using System.ComponentModel.DataAnnotations;

namespace DON.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }  // Login username

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        //public string? Department { get; set; }  // only for students
    }

}
