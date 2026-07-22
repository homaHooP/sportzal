using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppApi.Migrations
{
    /// <inheritdoc />
    public partial class SessionTweaks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "Sessions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "Sessions");
        }
    }
}
