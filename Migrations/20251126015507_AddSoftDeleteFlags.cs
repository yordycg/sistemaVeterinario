using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "usuarios",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "mascotas",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsActivo",
                table: "clientes",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "mascotas");

            migrationBuilder.DropColumn(
                name: "EsActivo",
                table: "clientes");
        }
    }
}
