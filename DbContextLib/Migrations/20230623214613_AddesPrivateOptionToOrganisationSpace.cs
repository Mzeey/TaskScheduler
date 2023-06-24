using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContextLib.Migrations
{
    public partial class AddesPrivateOptionToOrganisationSpace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "OrganisationSpaces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "OrganisationSpaces");
        }
    }
}
