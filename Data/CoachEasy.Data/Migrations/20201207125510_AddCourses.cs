using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachEasy.Data.Migrations
{
    public partial class AddCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachClients");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "ContactForms");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "ContactForms",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "ContactForms",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ContactForms",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ContactForms",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    StarDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ForPosition = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CoachId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseClients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    CourseId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseClients_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CourseId",
                table: "Clients",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClients_ClientId",
                table: "CourseClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClients_CourseId",
                table: "CourseClients",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseClients_IsDeleted",
                table: "CourseClients",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CoachId",
                table: "Courses",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_IsDeleted",
                table: "Courses",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Courses_CourseId",
                table: "Clients",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Courses_CourseId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "CourseClients");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CourseId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ContactForms");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "ContactForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CoachClients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CoachId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoachClients_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_ClientId",
                table: "CoachClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_CoachId",
                table: "CoachClients",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_CoachClients_IsDeleted",
                table: "CoachClients",
                column: "IsDeleted");
        }
    }
}
