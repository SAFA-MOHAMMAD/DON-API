// DON/ViewModels/CourseDto.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class CourseDto
{
    [Required]
    public string CourseCode { get; set; }

    [Required]
    public string CourseName { get; set; }

    [Required]
    public string Department { get; set; }

    [Required]
    public int InstructorId { get; set; }

    public int? SemesterId { get; set; }
    public int? Credits { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public string? Duration { get; set; }     // we'll parse this to TimeSpan
    public IFormFile? ImagePath { get; set; }  // file upload
}
