using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskAssignmentTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LearningTaskTitle",
                table: "TaskAssignments",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MentorName",
                table: "TaskAssignments",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraineeName",
                table: "TaskAssignments",
                type: "longtext",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$QnkYK8ITojLlx09j0/OkueKcevkjAnGJI8X1UxtXJ0os/ZFE6nGei");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LearningTaskTitle",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "MentorName",
                table: "TaskAssignments");

            migrationBuilder.DropColumn(
                name: "TraineeName",
                table: "TaskAssignments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$vYKM4UYCLoHp4/CUyR3Kruek.a.Y8AEZ/DC292IRdj02R2C30nrwy");
        }
    }
}
