using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_Hotel_V2.Migrations
{
    public partial class reservaFecha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fechaDeCreacion",
                table: "Reservas",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fechaDeCreacion",
                table: "Reservas");
        }
    }
}
