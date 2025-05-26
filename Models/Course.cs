using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DON.Models
{
    public class Course
    {
        public int Id { get; set; }  // Primary Key

        public string? CourseCode { get; set; }  // e.g. "MATH101"
        public string CourseName { get; set; }  // e.g. "Calculus I"
        public string Department { get; set; }

        // Foreign Key to Semester (optional if course exists before being assigned a semester)
        public int? SemesterId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public Semester Semester { get; set; }

        // Foreign Key to Instructor
        public int InstructorId { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public InstructorProfile Instructor { get; set; }

        public int? Credits { get; set; }
        public string? Description { get; set; }
        public string? Level { get; set; }  // e.g. Beginner, Intermediate, Advanced
        public TimeSpan? Duration { get; set; }  // e.g. course total hours
        public string? ImagePath { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

}
