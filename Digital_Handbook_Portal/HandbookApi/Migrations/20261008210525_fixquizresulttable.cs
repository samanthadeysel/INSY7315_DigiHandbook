using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandbookApi.Migrations
{
    /// <inheritdoc />
    public partial class fixquizresulttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResult_User_userId",
                table: "QuizResult");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuizResult");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "QuizResult",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResult_userId",
                table: "QuizResult",
                newName: "IX_QuizResult_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "QuizResult",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResult_User_UserId",
                table: "QuizResult",
                column: "UserId",
                principalTable: "User",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizResult_User_UserId",
                table: "QuizResult");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuizResult",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_QuizResult_UserId",
                table: "QuizResult",
                newName: "IX_QuizResult_userId");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "QuizResult",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "QuizResult",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizResult_User_userId",
                table: "QuizResult",
                column: "userId",
                principalTable: "User",
                principalColumn: "userId");
        }
    }
}
