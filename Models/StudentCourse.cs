namespace DON.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public StudentProfile Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int? SemesterId { get; set; }  // Nullable to allow extra/non-semester courses
        public Semester? Semester { get; set; }

        public int? Hours { get; set; }
    }
}
