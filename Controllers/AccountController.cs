using DON.Context;
using DON.Models;
using DON.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DON.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpPost]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                UserType = model.UserType
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            string imagePath = "images/default.jpg";
            if (result.Succeeded)
            {
                if (user.UserType == UserType.Student)
                {
                    var studentProfile = new StudentProfile
                    {
                        ApplicationUserId = user.Id,
                        Department = "N/A",
                        ImagePath = imagePath
                    };
                    _context.StudentProfiles.Add(studentProfile);
                }
                else if (user.UserType == UserType.Instructor)
                {
                    var instructorProfile = new InstructorProfile
                    {
                        ApplicationUserId = user.Id,
                        ImagePath = imagePath
                    };
                    _context.InstructorProfiles.Add(instructorProfile);
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Registration successful", userId = user.Id });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password.");

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful",
                    userId = user.Id,
                    userType = user.UserType,
                    ImagePath = user.UserType == UserType.Student
                    ? _context.StudentProfiles.FirstOrDefault(p => p.ApplicationUserId == user.Id)?.ImagePath
                    : _context.InstructorProfiles.FirstOrDefault(p => p.ApplicationUserId == user.Id)?.ImagePath
                });
            }

            return Unauthorized("Invalid username or password.");
        }


        [HttpPut("upload-image/{userId}")]
        public async Task<IActionResult> UploadProfileImage(string userId, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            var fileName = Path.GetFileName(image.FileName);
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            string relativePath = $"images/{fileName}";

            if (user.UserType == UserType.Student)
            {
                var student = _context.StudentProfiles.FirstOrDefault(s => s.ApplicationUserId == userId);
                if (student == null) return NotFound("Student profile not found.");
                student.ImagePath = relativePath;
            }
            else if (user.UserType == UserType.Instructor)
            {
                var instructor = _context.InstructorProfiles.FirstOrDefault(i => i.ApplicationUserId == userId);
                if (instructor == null) return NotFound("Instructor profile not found.");
                instructor.ImagePath = relativePath;
            }

            await _context.SaveChangesAsync();
            return Ok(new { imagePath = relativePath });
        }

        [HttpPut("set-default-images")]
        public IActionResult SetDefaultImages()
        {
            var defaultImagePath = "images/default.jpg";

            var studentsToUpdate = _context.StudentProfiles
                .Where(s => s.ImagePath == null)
                .ToList();

            foreach (var student in studentsToUpdate)
            {
                student.ImagePath = defaultImagePath;
            }

            var instructorsToUpdate = _context.InstructorProfiles
                .Where(i => i.ImagePath == null)
                .ToList();

            foreach (var instructor in instructorsToUpdate)
            {
                instructor.ImagePath = defaultImagePath;
            }

            _context.SaveChanges();

            return Ok(new
            {
                updatedStudents = studentsToUpdate.Count,
                updatedInstructors = instructorsToUpdate.Count
            });
        }


    }

}
