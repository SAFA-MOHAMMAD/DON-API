namespace DON.Models
{
    public class Semester
    {
        public int Id { get; set; }

        public string Name { get; set; } // "Fall 2022", "Spring 2023", etc.

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
