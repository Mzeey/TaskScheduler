using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContextLib.Migrations
{
    public partial class AddsCreatorIdToOrganisationSpace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "OrganisationSpaces",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationSpaces_CreatorId",
                table: "OrganisationSpaces",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganisationSpaces_Users_CreatorId",
                table: "OrganisationSpaces",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganisationSpaces_Users_CreatorId",
                table: "OrganisationSpaces");

            migrationBuilder.DropIndex(
                name: "IX_OrganisationSpaces_CreatorId",
                table: "OrganisationSpaces");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OrganisationSpaces");
        }
    }
}
