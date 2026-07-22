using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppApi.Migrations
{
    /// <inheritdoc />
    public partial class GymSoftDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Gyms",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Gyms");
        }
    }
}
