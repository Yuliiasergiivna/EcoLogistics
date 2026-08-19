using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGeo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_persos_Localite_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.DropForeignKey(
                name: "FK_Localite_CommuneBXL_Id_commune",
                table: "Localite");

            migrationBuilder.DropForeignKey(
                name: "FK_Localite_Pays_Id_pays",
                table: "Localite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Localite",
                table: "Localite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommuneBXL",
                table: "CommuneBXL");

            migrationBuilder.RenameTable(
                name: "Localite",
                newName: "Localites");

            migrationBuilder.RenameTable(
                name: "CommuneBXL",
                newName: "CommuneBXLS");

            migrationBuilder.RenameIndex(
                name: "IX_Localite_Id_pays",
                table: "Localites",
                newName: "IX_Localites_Id_pays");

            migrationBuilder.RenameIndex(
                name: "IX_Localite_Id_commune",
                table: "Localites",
                newName: "IX_Localites_Id_commune");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Localites",
                table: "Localites",
                column: "Id_localite");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommuneBXLS",
                table: "CommuneBXLS",
                column: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite",
                principalTable: "Localites",
                principalColumn: "Id_localite",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Localites_CommuneBXLS_Id_commune",
                table: "Localites",
                column: "Id_commune",
                principalTable: "CommuneBXLS",
                principalColumn: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Localites_Pays_Id_pays",
                table: "Localites",
                column: "Id_pays",
                principalTable: "Pays",
                principalColumn: "Id_pays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_persos_Localites_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.DropForeignKey(
                name: "FK_Localites_CommuneBXLS_Id_commune",
                table: "Localites");

            migrationBuilder.DropForeignKey(
                name: "FK_Localites_Pays_Id_pays",
                table: "Localites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Localites",
                table: "Localites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommuneBXLS",
                table: "CommuneBXLS");

            migrationBuilder.RenameTable(
                name: "Localites",
                newName: "Localite");

            migrationBuilder.RenameTable(
                name: "CommuneBXLS",
                newName: "CommuneBXL");

            migrationBuilder.RenameIndex(
                name: "IX_Localites_Id_pays",
                table: "Localite",
                newName: "IX_Localite_Id_pays");

            migrationBuilder.RenameIndex(
                name: "IX_Localites_Id_commune",
                table: "Localite",
                newName: "IX_Localite_Id_commune");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Localite",
                table: "Localite",
                column: "Id_localite");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommuneBXL",
                table: "CommuneBXL",
                column: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_persos_Localite_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite",
                principalTable: "Localite",
                principalColumn: "Id_localite",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Localite_CommuneBXL_Id_commune",
                table: "Localite",
                column: "Id_commune",
                principalTable: "CommuneBXL",
                principalColumn: "Id_commune");

            migrationBuilder.AddForeignKey(
                name: "FK_Localite_Pays_Id_pays",
                table: "Localite",
                column: "Id_pays",
                principalTable: "Pays",
                principalColumn: "Id_pays");
        }
    }
}
