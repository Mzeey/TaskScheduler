using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContextLib.Migrations
{
    public partial class AddsNewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganisationSpaceId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationSpaceId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrganisationSpaces",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationSpaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskItemComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskItemId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItemComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItemComments_Tasks_TaskItemId",
                        column: x => x.TaskItemId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskItemComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganisationUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganisationSpaceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganisationUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganisationUserRoles_OrganisationSpaces_OrganisationSpaceId",
                        column: x => x.OrganisationSpaceId,
                        principalTable: "OrganisationSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganisationUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganisationSpaceId",
                table: "Users",
                column: "OrganisationSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OrganisationSpaceId",
                table: "Tasks",
                column: "OrganisationSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUserRoles_OrganisationSpaceId",
                table: "OrganisationUserRoles",
                column: "OrganisationSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUserRoles_RoleId",
                table: "OrganisationUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganisationUserRoles_UserId",
                table: "OrganisationUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemComments_TaskItemId",
                table: "TaskItemComments",
                column: "TaskItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItemComments_UserId",
                table: "TaskItemComments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_OrganisationSpaces_OrganisationSpaceId",
                table: "Tasks",
                column: "OrganisationSpaceId",
                principalTable: "OrganisationSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrganisationSpaces_OrganisationSpaceId",
                table: "Users",
                column: "OrganisationSpaceId",
                principalTable: "OrganisationSpaces",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_OrganisationSpaces_OrganisationSpaceId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_OrganisationSpaces_OrganisationSpaceId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "OrganisationUserRoles");

            migrationBuilder.DropTable(
                name: "TaskItemComments");

            migrationBuilder.DropTable(
                name: "OrganisationSpaces");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganisationSpaceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OrganisationSpaceId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "OrganisationSpaceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OrganisationSpaceId",
                table: "Tasks");
        }
    }
}
