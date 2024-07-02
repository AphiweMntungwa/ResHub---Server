using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResHub.Migrations
{
    /// <inheritdoc />
    public partial class identityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "Residents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Residents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "Residents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "Residents",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "Residents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Residents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "Residents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Residents");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "Residents");
        }
    }
}
