using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TraineeManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubmissionId = table.Column<int>(type: "int", nullable: true),
                    MentorId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Feedback = table.Column<string>(type: "longtext", nullable: true),
                    ReviewStatus = table.Column<string>(type: "longtext", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TaskAssignmentId = table.Column<int>(type: "int", nullable: true),
                    ReviewedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Mentors_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_TaskAssignments_TaskAssignmentId",
                        column: x => x.TaskAssignmentId,
                        principalTable: "TaskAssignments",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$bMSNMxlU/Rtyle.cmN8rl.vw7DkElSIqrxod2qKabxh7Tm8hh2vhy");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MentorId",
                table: "Reviews",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_SubmissionId",
                table: "Reviews",
                column: "SubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TaskAssignmentId",
                table: "Reviews",
                column: "TaskAssignmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$UcL5ERSQ5hkdedOsU5OZueKDpJvCGObOJ/dFEp3AFUvHAMCtH4qYq");
        }
    }
}
