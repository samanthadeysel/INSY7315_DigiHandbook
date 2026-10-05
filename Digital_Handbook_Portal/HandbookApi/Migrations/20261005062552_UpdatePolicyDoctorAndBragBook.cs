using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HandbookApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyDoctorAndBragBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "doctorId",
                table: "Policy",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Policy_doctorId",
                table: "Policy",
                column: "doctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Policy_Doctor_doctorId",
                table: "Policy",
                column: "doctorId",
                principalTable: "Doctor",
                principalColumn: "doctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policy_Doctor_doctorId",
                table: "Policy");

            migrationBuilder.DropIndex(
                name: "IX_Policy_doctorId",
                table: "Policy");

            migrationBuilder.DropColumn(
                name: "doctorId",
                table: "Policy");
        }
    }
}
