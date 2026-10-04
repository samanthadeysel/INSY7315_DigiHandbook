using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandbookApi.Migrations
{
    /// <inheritdoc />
    public partial class removeimgurlfrombrag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"BragBook\" DROP COLUMN IF EXISTS \"imageUrl\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "BragBook",
                type: "text",
                nullable: true);
        }
    }
}
