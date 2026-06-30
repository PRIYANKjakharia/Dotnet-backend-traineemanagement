using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$DQT/Ytp62Z2D7ZfZ2vFjOe0mFfx..dovHenwqkAUTFm1HY2RptKfG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$C96pUSAI0Pt0NiRDZo9wiuIy2X8R65XGrsbfqgu3lvXHT7Z5nmiOq");
        }
    }
}
