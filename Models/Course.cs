namespace DON.Models
{
    public class Course
    {
        public int Id { get; set; }  // auto-incremented PK
        public string CourseCode { get; set; }  // e.g. "MATH101"
        public string CourseName { get; set; }  // e.g. "Calculus I"
        public string Department { get; set; }
        public string? Semester { get; set; }
        public string InstructorName { get; set; }
        public int? Credits { get; set; }
        public string? Description { get; set; }
        public string? Level { get; set; }
        public TimeSpan? Duration { get; set; }
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
