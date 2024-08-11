using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResHub.Migrations
{
    /// <inheritdoc />
    public partial class Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventResidents");

            migrationBuilder.AddColumn<int>(
                name: "ResidenceId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ResidenceId",
                table: "Events",
                column: "ResidenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Residence_ResidenceId",
                table: "Events",
                column: "ResidenceId",
                principalTable: "Residence",
                principalColumn: "ResId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Residence_ResidenceId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ResidenceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ResidenceId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventResidents",
                columns: table => new
                {
                    ResidenceId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventResidents", x => new { x.ResidenceId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventResidents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventResidents_Residence_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residence",
                        principalColumn: "ResId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EventResidents_EventId",
                table: "EventResidents",
                column: "EventId");
        }
    }
}
