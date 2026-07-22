using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppApi.Migrations
{
    /// <inheritdoc />
    public partial class MembershipTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "TotalSessions",
                table: "Memberships");

            migrationBuilder.AddColumn<DateTime>(
                name: "NullifiedAt",
                table: "Memberships",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NullifiedAt",
                table: "Memberships");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Memberships",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalSessions",
                table: "Memberships",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
