using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class SeedEstadoConsultaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "motivo",
                table: "consultas",
                type: "text",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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

            migrationBuilder.InsertData(
                table: "estado_consultas",
                columns: new[] { "id_estado_consulta", "nombre_estado" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "En Progreso" },
                    { 3, "Finalizada" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "estado_consultas",
                keyColumn: "id_estado_consulta",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "estado_consultas",
                keyColumn: "id_estado_consulta",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "estado_consultas",
                keyColumn: "id_estado_consulta",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "motivo",
                table: "consultas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "diagnostico",
                table: "consultas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 1000);
        }
    }
}
