using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HandbookApi.Migrations
{
    /// <inheritdoc />
    public partial class addquizresulltstableandeditUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FragmentVisit_UserSession_UserSessionSessionId",
                table: "FragmentVisit");

            migrationBuilder.DropIndex(
                name: "IX_FragmentVisit_UserSessionSessionId",
                table: "FragmentVisit");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "UserSessionSessionId",
                table: "FragmentVisit");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserSession",
                newName: "userId");

            migrationBuilder.Sql(
                "ALTER TABLE \"UserSession\" " +
                "ALTER COLUMN \"userId\" " +
                "TYPE integer " +
                "USING \"userId\"::integer;");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuizResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: true),
                    QuizId = table.Column<int>(type: "integer", nullable: false),
                    ScorePercentage = table.Column<int>(type: "integer", nullable: false),
                    CpdPointsAwarded = table.Column<int>(type: "integer", nullable: false),
                    IsPassed = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizResult_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "quizId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizResult_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_userId",
                table: "UserSession",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_FragmentVisit_SessionId",
                table: "FragmentVisit",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResult_QuizId",
                table: "QuizResult",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResult_userId",
                table: "QuizResult",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_FragmentVisit_UserSession_SessionId",
                table: "FragmentVisit",
                column: "SessionId",
                principalTable: "UserSession",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSession_User_userId",
                table: "UserSession",
                column: "userId",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FragmentVisit_UserSession_SessionId",
                table: "FragmentVisit");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSession_User_userId",
                table: "UserSession");

            migrationBuilder.DropTable(
                name: "QuizResult");

            migrationBuilder.DropIndex(
                name: "IX_UserSession_userId",
                table: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_FragmentVisit_SessionId",
                table: "FragmentVisit");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "UserSession",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSession",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "UserSession",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserSessionSessionId",
                table: "FragmentVisit",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FragmentVisit_UserSessionSessionId",
                table: "FragmentVisit",
                column: "UserSessionSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FragmentVisit_UserSession_UserSessionSessionId",
                table: "FragmentVisit",
                column: "UserSessionSessionId",
                principalTable: "UserSession",
                principalColumn: "SessionId");
        }
    }
}
