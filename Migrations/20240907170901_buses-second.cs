using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResHub.Migrations
{
    /// <inheritdoc />
    public partial class busessecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bus_AspNetUsers_LastUpdatedByUserId",
                table: "Bus");

            migrationBuilder.DropIndex(
                name: "IX_Bus_LastUpdatedByUserId",
                table: "Bus");

            migrationBuilder.AlterColumn<string>(
                name: "LastUpdatedByUserId",
                table: "Bus",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastUpdatedByUserId",
                table: "Bus",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Bus_LastUpdatedByUserId",
                table: "Bus",
                column: "LastUpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bus_AspNetUsers_LastUpdatedByUserId",
                table: "Bus",
                column: "LastUpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
