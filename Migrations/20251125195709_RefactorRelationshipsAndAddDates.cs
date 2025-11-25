using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistemaVeterinario.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRelationshipsAndAddDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__mascotas__id_esp__4D94879B",
                table: "mascotas");

            migrationBuilder.DropIndex(
                name: "IX_mascotas_id_especie",
                table: "mascotas");

            migrationBuilder.DropColumn(
                name: "id_especie",
                table: "mascotas");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaRegistro",
                table: "usuarios",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AlterColumn<string>(
                name: "medicamento",
                table: "tratamientos",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "tratamientos",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaRegistro",
                table: "tratamientos",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "id_especie",
                table: "razas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaRegistro",
                table: "mascotas",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaCreacion",
                table: "consultas",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaRegistro",
                table: "clientes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 1,
                column: "id_especie",
                value: 1);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 2,
                column: "id_especie",
                value: 1);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 3,
                column: "id_especie",
                value: 2);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 4,
                column: "id_especie",
                value: 2);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 5,
                column: "id_especie",
                value: 3);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 6,
                column: "id_especie",
                value: 4);

            migrationBuilder.UpdateData(
                table: "razas",
                keyColumn: "id_raza",
                keyValue: 7,
                column: "id_especie",
                value: 5);

            migrationBuilder.CreateIndex(
                name: "IX_razas_id_especie",
                table: "razas",
                column: "id_especie");

            migrationBuilder.AddForeignKey(
                name: "FK_razas_especies",
                table: "razas",
                column: "id_especie",
                principalTable: "especies",
                principalColumn: "id_especie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_razas_especies",
                table: "razas");

            migrationBuilder.DropIndex(
                name: "IX_razas_id_especie",
                table: "razas");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "tratamientos");

            migrationBuilder.DropColumn(
                name: "id_especie",
                table: "razas");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "mascotas");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "consultas");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "clientes");

            migrationBuilder.AlterColumn<string>(
                name: "medicamento",
                table: "tratamientos",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldUnicode: false,
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "tratamientos",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "id_especie",
                table: "mascotas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_mascotas_id_especie",
                table: "mascotas",
                column: "id_especie");

            migrationBuilder.AddForeignKey(
                name: "FK__mascotas__id_esp__4D94879B",
                table: "mascotas",
                column: "id_especie",
                principalTable: "especies",
                principalColumn: "id_especie");
        }
    }
}
