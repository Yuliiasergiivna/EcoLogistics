using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDonneesPerso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.AlterColumn<int>(
                name: "Id_localite",
                table: "Donnees_persos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_licenciement",
                table: "Donnees_persos",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite",
                principalTable: "Localites",
                principalColumn: "Id_localite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.AlterColumn<int>(
                name: "Id_localite",
                table: "Donnees_persos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_licenciement",
                table: "Donnees_persos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite",
                principalTable: "Localites",
                principalColumn: "Id_localite",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
