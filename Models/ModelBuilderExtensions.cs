using Microsoft.EntityFrameworkCore;

namespace DON.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed (this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Semester>().HasData(
                new Semester
                {
                    Id = 1,
                    Name = "Fall 2023",
                    StartDate = new DateTime(2023, 9, 1),
                    EndDate = new DateTime(2023, 12, 31)
                },
                new Semester
                {
                    Id = 2,
                    Name = "Spring 2024",
                    StartDate = new DateTime(2024, 1, 15),
                    EndDate = new DateTime(2024, 5, 15)
                }
            );
        }
    }
}
