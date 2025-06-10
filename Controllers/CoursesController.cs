using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DON.Context;
using DON.Models;

namespace DON.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Course>> PostCourse([FromForm] CourseDto dto)
        {
            // Handle the image file
            string imagePath = "/images/default-course.png";
            if (dto.ImagePath?.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploads);

                var fileName = Path.GetFileName(dto.ImagePath.FileName);
                var fullPath = Path.Combine(uploads, fileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.ImagePath.CopyToAsync(stream);

                imagePath = $"/images/{fileName}";
            }

            // Parse duration if provided
            TimeSpan? duration = null;
            if (!string.IsNullOrWhiteSpace(dto.Duration))
                duration = TimeSpan.Parse(dto.Duration);

            // Map to your entity
            var course = new Course
            {
                CourseCode = dto.CourseCode,
                CourseName = dto.CourseName,
                Department = dto.Department,
                InstructorId = dto.InstructorId,
                SemesterId = dto.SemesterId,
                Credits = dto.Credits,
                Description = dto.Description,
                Level = dto.Level,
                Duration = duration,
                ImagePath = imagePath
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }


        /// <summary>
        /// Gets all courses taught by a specific instructor.
        /// </summary>
        /// <param name="instructorId">The ID of the instructor.</param>
        /// <returns>A list of courses.</returns>
        [HttpGet("byInstructor/{instructorId}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByInstructor(int instructorId)
        {
            // Check if the instructor exists (optional, but good practice)
            var instructorExists = await _context.InstructorProfiles.AnyAsync(i => i.Id == instructorId);
            if (!instructorExists)
            {
                return NotFound($"Instructor with ID {instructorId} not found.");
            }

            // Retrieve courses for the specified instructor
            var courses = await _context.Courses
                                        .Where(c => c.InstructorId == instructorId)
                                        .ToListAsync();

            if (!courses.Any())
            {
                return NotFound($"No courses found for instructor with ID {instructorId}.");
            }

            return Ok(courses);
        }

        /// <summary>
        /// Gets all courses taught by a specific instructor using their ApplicationUser ID.
        /// </summary>
        /// <param name="applicationUserId">The ApplicationUser ID (string GUID) of the instructor.</param>
        /// <returns>A list of courses.</returns>
        [HttpGet("byApplicationUser/{applicationUserId}")] // Changed route name to be explicit
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByApplicationUserId(string applicationUserId)
        {
            // 1. Find the InstructorProfile associated with the given ApplicationUserId
            var instructorProfile = await _context.InstructorProfiles
                                                .FirstOrDefaultAsync(ip => ip.ApplicationUserId == applicationUserId);

            if (instructorProfile == null)
            {
                return NotFound($"Instructor profile not found for ApplicationUser ID: {applicationUserId}.");
            }

            // 2. Use the found InstructorProfile's Id to retrieve courses
            var courses = await _context.Courses
                                        .Where(c => c.InstructorId == instructorProfile.Id)
                                        .ToListAsync();

            if (!courses.Any())
            {
                return NotFound($"No courses found for instructor with ApplicationUser ID {applicationUserId}.");
            }

            return Ok(courses);
        }


        /// <summary>
        /// Gets all courses enrolled by a specific student using their ApplicationUser ID.
        /// </summary>
        /// <param name="applicationUserId">The ApplicationUser ID (string GUID) of the student.</param>
        /// <returns>A list of courses the student is enrolled in.</returns>
        [HttpGet("byStudentApplicationUser/{applicationUserId}")] // New route for students
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesByStudentApplicationUserId(string applicationUserId)
        {
            // 1. Find the StudentProfile associated with the given ApplicationUserId
            var studentProfile = await _context.StudentProfiles
                                             .FirstOrDefaultAsync(sp => sp.ApplicationUserId == applicationUserId);

            if (studentProfile == null)
            {
                return NotFound($"Student profile not found for ApplicationUser ID: {applicationUserId}.");
            }

            // 2. Use the found StudentProfile's Id to retrieve courses through StudentCourse
            var courses = await _context.StudentCourses
                                        .Where(sc => sc.StudentId == studentProfile.Id)
                                        .Select(sc => sc.Course) // Select the actual Course object
                                        .ToListAsync();

            if (!courses.Any())
            {
                // It's possible a student has no courses enrolled yet
                return Ok(new List<Course>()); // Return empty list instead of NotFound if student exists but no courses
            }

            return Ok(courses);
        }

    }
}
