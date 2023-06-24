using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContextLib.Migrations
{
    public partial class UpdatesOrganisationUserSpaceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserRoles_OrganisationSpaces_OrganisationSpaceId",
                table: "OrganisationUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserRoles_Roles_RoleId",
                table: "OrganisationUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserRoles_Users_UserId",
                table: "OrganisationUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationUserRoles",
                table: "OrganisationUserRoles");

            migrationBuilder.RenameTable(
                name: "OrganisationUserRoles",
                newName: "OrganisationUserSpaces");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserRoles_UserId",
                table: "OrganisationUserSpaces",
                newName: "IX_OrganisationUserSpaces_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserRoles_RoleId",
                table: "OrganisationUserSpaces",
                newName: "IX_OrganisationUserSpaces_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserRoles_OrganisationSpaceId",
                table: "OrganisationUserSpaces",
                newName: "IX_OrganisationUserSpaces_OrganisationSpaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationUserSpaces",
                table: "OrganisationUserSpaces",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserSpaces_OrganisationSpaces_OrganisationSpaceId",
                table: "OrganisationUserSpaces",
                column: "OrganisationSpaceId",
                principalTable: "OrganisationSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserSpaces_Roles_RoleId",
                table: "OrganisationUserSpaces",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserSpaces_Users_UserId",
                table: "OrganisationUserSpaces",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserSpaces_OrganisationSpaces_OrganisationSpaceId",
                table: "OrganisationUserSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserSpaces_Roles_RoleId",
                table: "OrganisationUserSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationUserSpaces_Users_UserId",
                table: "OrganisationUserSpaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganisationUserSpaces",
                table: "OrganisationUserSpaces");

            migrationBuilder.RenameTable(
                name: "OrganisationUserSpaces",
                newName: "OrganisationUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserSpaces_UserId",
                table: "OrganisationUserRoles",
                newName: "IX_OrganisationUserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserSpaces_RoleId",
                table: "OrganisationUserRoles",
                newName: "IX_OrganisationUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganisationUserSpaces_OrganisationSpaceId",
                table: "OrganisationUserRoles",
                newName: "IX_OrganisationUserRoles_OrganisationSpaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganisationUserRoles",
                table: "OrganisationUserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserRoles_OrganisationSpaces_OrganisationSpaceId",
                table: "OrganisationUserRoles",
                column: "OrganisationSpaceId",
                principalTable: "OrganisationSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserRoles_Roles_RoleId",
                table: "OrganisationUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationUserRoles_Users_UserId",
                table: "OrganisationUserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
