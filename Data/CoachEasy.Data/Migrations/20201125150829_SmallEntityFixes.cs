using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachEasy.Data.Migrations
{
    public partial class SmallEntityFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Pictures_PictureId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_PictureId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "PictureId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Workouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachId",
                table: "Workouts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_ClientId",
                table: "Workouts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_CoachId",
                table: "Workouts",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Clients_ClientId",
                table: "Workouts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Coaches_CoachId",
                table: "Workouts",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Clients_ClientId",
                table: "Workouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Coaches_CoachId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_ClientId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_CoachId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "Workouts");

            migrationBuilder.AddColumn<string>(
                name: "PictureId",
                table: "Players",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PictureId",
                table: "Players",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Pictures_PictureId",
                table: "Players",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
