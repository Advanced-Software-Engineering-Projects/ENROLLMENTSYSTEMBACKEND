using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENROLLMENTSYSTEMBACKEND.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredCredits",
                table: "Programs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCredits",
                table: "Programs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RequiredCredits",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "TotalCredits",
                table: "Programs");
        }
    }
}
