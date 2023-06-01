using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_Hotel_V2.Migrations
{
    public partial class personauq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cuil",
                table: "Afiliados",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "UqCuil",
                table: "Afiliados",
                column: "Cuil",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UqCuil",
                table: "Afiliados");

            migrationBuilder.AlterColumn<string>(
                name: "Cuil",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
