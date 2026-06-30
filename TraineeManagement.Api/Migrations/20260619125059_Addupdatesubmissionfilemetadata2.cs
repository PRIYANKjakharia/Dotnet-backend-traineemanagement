using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addupdatesubmissionfilemetadata2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "SubmissionId1",
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
                value: "$2a$11$u1lbmVeJN3gxjHM8t5g0OOP6oiyo/S7m3Wut/C/ysq2BdGpGiFjx2");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id");
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
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                value: "$2a$11$fh06o111m4s9fE5b7t9YtelYwxDSEMk./arZY71Wc5PfM3Z0FWnpm");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionFiles_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId",
                table: "SubmissionFiles",
                column: "SubmissionId",
                principalTable: "Submissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubmissionFiles_Submissions_SubmissionId1",
                table: "SubmissionFiles",
                column: "SubmissionId1",
                principalTable: "Submissions",
                principalColumn: "Id");
        }
    }
}
