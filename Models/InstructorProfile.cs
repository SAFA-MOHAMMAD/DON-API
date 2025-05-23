namespace DON.Models
{
    public class InstructorProfile
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Course> Courses { get; set; }
        public string? ImagePath { get; set; }
    }
}
