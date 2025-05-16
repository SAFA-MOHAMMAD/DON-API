namespace DON.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
