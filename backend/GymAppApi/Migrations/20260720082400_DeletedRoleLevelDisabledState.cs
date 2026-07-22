using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymAppApi.Migrations
{
    /// <inheritdoc />
    public partial class DeletedRoleLevelDisabledState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGymManager_AspNetUsers_UserId",
                table: "UserGymManager");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGymManager_Gyms_GymId",
                table: "UserGymManager");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGymManager",
                table: "UserGymManager");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "UserTrainers");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "UserClients");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "UserGymManager");

            migrationBuilder.RenameTable(
                name: "UserGymManager",
                newName: "UserGymManagers");

            migrationBuilder.RenameIndex(
                name: "IX_UserGymManager_GymId",
                table: "UserGymManagers",
                newName: "IX_UserGymManagers_GymId");

            migrationBuilder.AddColumn<DateTime>(
                name: "WasDeactivated",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGymManagers",
                table: "UserGymManagers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGymManagers_AspNetUsers_UserId",
                table: "UserGymManagers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGymManagers_Gyms_GymId",
                table: "UserGymManagers",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGymManagers_AspNetUsers_UserId",
                table: "UserGymManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGymManagers_Gyms_GymId",
                table: "UserGymManagers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGymManagers",
                table: "UserGymManagers");

            migrationBuilder.DropColumn(
                name: "WasDeactivated",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "UserGymManagers",
                newName: "UserGymManager");

            migrationBuilder.RenameIndex(
                name: "IX_UserGymManagers_GymId",
                table: "UserGymManager",
                newName: "IX_UserGymManager_GymId");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "WasDeactivated",
                table: "UserGymManager",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGymManager",
                table: "UserGymManager",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGymManager_AspNetUsers_UserId",
                table: "UserGymManager",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGymManager_Gyms_GymId",
                table: "UserGymManager",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
