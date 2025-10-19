using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class MakeDiagnosticoNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "diagnostico",
                table: "consultas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 1000);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "diagnostico",
                table: "consultas",
                type: "text",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
