using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Digital_Handbook_Portal.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BragBook",
                columns: table => new
                {
                    bragId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    recipientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datePosted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BragBook", x => x.bragId);
                });

            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    eventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eventDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.eventId);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                columns: table => new
                {
                    policyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contentSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.policyId);
                });

            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    quizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    questions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    score = table.Column<int>(type: "int", nullable: false),
                    totalQuestions = table.Column<int>(type: "int", nullable: false),
                    estimateTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.quizId);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    resourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    linkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Title);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BragBook");

            migrationBuilder.DropTable(
                name: "Community");

            migrationBuilder.DropTable(
                name: "Policy");

            migrationBuilder.DropTable(
                name: "Quiz");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
