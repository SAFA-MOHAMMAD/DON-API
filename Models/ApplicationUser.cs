using Microsoft.AspNetCore.Identity;

namespace DON.Models
{
    public class ApplicationUser : IdentityUser
    {
    public UserType UserType { get; set; }

    // Navigation properties
    public StudentProfile? StudentProfile { get; set; }
    public InstructorProfile? InstructorProfile { get; set; }    
    }

}
