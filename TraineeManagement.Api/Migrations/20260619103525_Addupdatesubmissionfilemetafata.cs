using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class Addupdatesubmissionfilemetafata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Checksum",
                table: "SubmissionFiles",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "SubmissionFiles",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UploadedBy",
                table: "SubmissionFiles",
                type: "longtext",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$4rGfD1UAmIL7QS.uIr0paeEkDe/jz.9vJSD3JysHidYmL3nYlkdbi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checksum",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "SubmissionFiles");

            migrationBuilder.DropColumn(
                name: "UploadedBy",
                table: "SubmissionFiles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$cQrEaP5lURkEbWmhM8yWQuaoo6kV3anOP0.u9WL3eiTD3hcUgdUGu");
        }
    }
}
