using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAndDonneesPerso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Donnees_persos_Id_perso",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Id_perso",
                table: "users",
                newName: "IX_users_Id_perso");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id_user");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Donnees_persos_Id_perso",
                table: "users",
                column: "Id_perso",
                principalTable: "Donnees_persos",
                principalColumn: "Id_perso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Donnees_persos_Id_perso",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_users_Id_perso",
                table: "Users",
                newName: "IX_Users_Id_perso");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id_user");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Donnees_persos_Id_perso",
                table: "Users",
                column: "Id_perso",
                principalTable: "Donnees_persos",
                principalColumn: "Id_perso");
        }
    }
}
