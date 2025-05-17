using DON.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DON.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<InstructorProfile> InstructorProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Semester)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentProfile>()
                .HasOne(sp => sp.ApplicationUser)
                .WithOne(u => u.StudentProfile)
                .HasForeignKey<StudentProfile>(sp => sp.ApplicationUserId);

            modelBuilder.Entity<InstructorProfile>()
                .HasOne(ip => ip.ApplicationUser)
                .WithOne(u => u.InstructorProfile)
                .HasForeignKey<InstructorProfile>(ip => ip.ApplicationUserId);
        }
    }
}
