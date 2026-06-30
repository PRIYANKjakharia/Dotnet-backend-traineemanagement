using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addupdatesubmissionfilemetadata1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "SubmissionFiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$fh06o111m4s9fE5b7t9YtelYwxDSEMk./arZY71Wc5PfM3Z0FWnpm");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "SubmissionFiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$UZT0swJXf4LMrf1c2Mgro.FsXpJGmOV9XxSnRfuEIePo0gtLtcVoa");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id");
        }
    }
}
