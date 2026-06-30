using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addupdatesubmissionfilemetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "SubmissionFiles",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "OrigionalFileName",
                table: "SubmissionFiles",
                newName: "OriginalFileName");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$UZT0swJXf4LMrf1c2Mgro.FsXpJGmOV9XxSnRfuEIePo0gtLtcVoa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "SubmissionFiles",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "SubmissionFiles",
                newName: "OrigionalFileName");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4rGfD1UAmIL7QS.uIr0paeEkDe/jz.9vJSD3JysHidYmL3nYlkdbi");
        }
    }
}
