using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLearningTaskTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpectedTechStack",
                table: "LearningTasks",
                type: "longtext",
                nullable: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$iTkktq/Jknoud9YJEJ34Gu.Y0je8YbiaBc4KNhX.FZVKVxEKvvYTG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedTechStack",
                table: "LearningTasks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$/gCKq5UfU/dnE/sIrPYt5uphmCkQPsFJUTQhsjzj9X71DfMJUdPr.");
        }
    }
}
