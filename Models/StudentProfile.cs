namespace DON.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string Department { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
