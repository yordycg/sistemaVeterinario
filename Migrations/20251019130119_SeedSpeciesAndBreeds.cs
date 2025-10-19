using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class SeedSpeciesAndBreeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "especies",
                columns: new[] { "id_especie", "nombre_especie" },
                values: new object[,]
                {
                    { 1, "Perro" },
                    { 2, "Gato" },
                    { 3, "Hámster" },
                    { 4, "Conejo" },
                    { 5, "Ave" }
                });

            migrationBuilder.InsertData(
                table: "razas",
                columns: new[] { "id_raza", "nombre_raza" },
                values: new object[,]
                {
                    { 1, "Labrador" },
                    { 2, "Poodle" },
                    { 3, "Siamés" },
                    { 4, "Persa" },
                    { 5, "Dorado" },
                    { 6, "Cabeza de León" },
                    { 7, "Canario" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "especies",
                keyColumn: "id_especie",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 7);
        }
    }
}
