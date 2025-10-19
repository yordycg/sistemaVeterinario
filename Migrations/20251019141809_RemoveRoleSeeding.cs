using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoleSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DeleteData(
            //     table: "roles",
            //     keyColumn: "id_rol",
            //     keyValue: 1);

            // migrationBuilder.DeleteData(
            //     table: "roles",
            //     keyColumn: "id_rol",
            //     keyValue: 2);

            // migrationBuilder.DeleteData(
            //     table: "roles",
            //     keyColumn: "id_rol",
            //     keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id_rol", "nombre_rol" },
                values: new object[,]
                {
                    { 1, "Administrador" },
                    { 2, "Veterinario" },
                    { 3, "Recepcionista" }
                });
        }
    }
}
