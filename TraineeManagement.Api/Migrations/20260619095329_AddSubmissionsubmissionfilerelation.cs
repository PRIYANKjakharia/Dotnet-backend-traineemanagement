using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionsubmissionfilerelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmissionId1",
                table: "SubmissionFiles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$cQrEaP5lURkEbWmhM8yWQuaoo6kV3anOP0.u9WL3eiTD3hcUgdUGu");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1",
                principalTable: "Submissions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4aMPEzGK/gZIT.elmYfbju.mntxN0W/BDwzChIjVvUcIW/q8fwVYO");
        }
    }
}
