using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DON.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "StudentProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "InstructorProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "StudentProfiles");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "InstructorProfiles");
        }
    }
}
