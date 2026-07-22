using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppApi.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_UserClients_ClientId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_UserClients_ClientId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_UserTrainers_TrainerId",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrainers",
                table: "UserTrainers");

            migrationBuilder.DropIndex(
                name: "IX_UserTrainers_UserId",
                table: "UserTrainers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClients",
                table: "UserClients");

            migrationBuilder.DropIndex(
                name: "IX_UserClients_UserId",
                table: "UserClients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTrainers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserClients");

            migrationBuilder.AddColumn<DateTime>(
                name: "WasDeactivated",
                table: "UserTrainers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WasDeactivated",
                table: "UserClients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GymId",
                table: "Memberships",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrainers",
                table: "UserTrainers",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClients",
                table: "UserClients",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "UserGymManager",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GymId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasDeactivated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGymManager", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserGymManager_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGymManager_Gyms_GymId",
                        column: x => x.GymId,
                        principalTable: "Gyms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_GymId",
                table: "Memberships",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGymManager_GymId",
                table: "UserGymManager",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_UserClients_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "UserClients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_Gyms_GymId",
                table: "Memberships",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_UserClients_ClientId",
                table: "Memberships",
                column: "ClientId",
                principalTable: "UserClients",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_UserTrainers_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "UserTrainers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_UserClients_ClientId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_Gyms_GymId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_UserClients_ClientId",
                table: "Memberships");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_UserTrainers_TrainerId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "UserGymManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrainers",
                table: "UserTrainers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClients",
                table: "UserClients");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_GymId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "UserTrainers");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "UserClients");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Memberships");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserTrainers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserClients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrainers",
                table: "UserTrainers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClients",
                table: "UserClients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainers_UserId",
                table: "UserTrainers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClients_UserId",
                table: "UserClients",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_UserClients_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "UserClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_UserClients_ClientId",
                table: "Memberships",
                column: "ClientId",
                principalTable: "UserClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_UserTrainers_TrainerId",
                table: "Sessions",
                column: "TrainerId",
                principalTable: "UserTrainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
