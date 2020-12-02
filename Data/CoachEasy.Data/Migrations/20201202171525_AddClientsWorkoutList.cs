using Microsoft.EntityFrameworkCore.Migrations;

namespace CoachEasy.Data.Migrations
{
    public partial class AddClientsWorkoutList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_Clients_ClientId",
                table: "Workouts");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_ClientId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Workouts");

            migrationBuilder.CreateTable(
                name: "WorkoutClients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    WorkoutId = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkoutClients_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutClients_ClientId",
                table: "WorkoutClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutClients_WorkoutId",
                table: "WorkoutClients",
                column: "WorkoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutClients");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Workouts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_ClientId",
                table: "Workouts",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_Clients_ClientId",
                table: "Workouts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
