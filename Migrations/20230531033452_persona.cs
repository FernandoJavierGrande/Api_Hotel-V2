using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_Hotel_V2.Migrations
{
    public partial class persona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NumAfiliado",
                table: "Afiliados",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UqNumAfiliado",
                table: "Afiliados",
                column: "NumAfiliado",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UqNumAfiliado",
                table: "Afiliados");

            migrationBuilder.AlterColumn<string>(
                name: "NumAfiliado",
                table: "Afiliados",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
