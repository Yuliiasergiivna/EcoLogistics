using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class AddConducteur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "N_plaque",
                table: "Conducteurs",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Conducteurs_Id_perso",
                table: "Conducteurs",
                column: "Id_perso");

            migrationBuilder.AddForeignKey(
                name: "FK_Conducteurs_Donnees_persos_Id_perso",
                table: "Conducteurs",
                column: "Id_perso",
                principalTable: "Donnees_persos",
                principalColumn: "Id_perso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conducteurs_Donnees_persos_Id_perso",
                table: "Conducteurs");

            migrationBuilder.DropIndex(
                name: "IX_Conducteurs_Id_perso",
                table: "Conducteurs");

            migrationBuilder.AlterColumn<string>(
                name: "N_plaque",
                table: "Conducteurs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
