using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addupdatesubmissionfilemetadata31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadedBy",
                table: "SubmissionFiles",
                newName: "UploadedByUser");

            migrationBuilder.AddColumn<int>(
                name: "UploadedByUserId",
                table: "SubmissionFiles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$hTThGZRYUa1Ju5eO6LYaTeMn66aSSz7F.f9BiYrqkADmG00iP3blO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadedByUserId",
                table: "SubmissionFiles");

            migrationBuilder.RenameColumn(
                name: "UploadedByUser",
                table: "SubmissionFiles",
                newName: "UploadedBy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$u1lbmVeJN3gxjHM8t5g0OOP6oiyo/S7m3Wut/C/ysq2BdGpGiFjx2");
        }
    }
}
