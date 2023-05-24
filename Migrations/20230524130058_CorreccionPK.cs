using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_Hotel_V2.Migrations
{
    public partial class CorreccionPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservaciones",
                table: "Reservaciones");

            migrationBuilder.DropIndex(
                name: "IX_Reservaciones_HabitacionId",
                table: "Reservaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservaciones",
                table: "Reservaciones",
                columns: new[] { "HabitacionId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservaciones_ReservaId",
                table: "Reservaciones",
                column: "ReservaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservaciones",
                table: "Reservaciones");

            migrationBuilder.DropIndex(
                name: "IX_Reservaciones_ReservaId",
                table: "Reservaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservaciones",
                table: "Reservaciones",
                columns: new[] { "ReservaId", "Fecha" });

            migrationBuilder.CreateIndex(
                name: "IX_Reservaciones_HabitacionId",
                table: "Reservaciones",
                column: "HabitacionId");
        }
    }
}
