using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlockGeo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conducteurs",
                newName: "Id_conducteur");

            migrationBuilder.CreateTable(
                name: "CommuneBXL",
                columns: table => new
                {
                    Id_commune = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Commune_principale = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sous_commune = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Nom_fr = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nom_nl = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommuneBXL", x => x.Id_commune);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pays",
                columns: table => new
                {
                    Id_pays = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_pays = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code_ISO = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.Id_pays);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Localite",
                columns: table => new
                {
                    Id_localite = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_localite = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code_postal = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Province = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_pays = table.Column<int>(type: "int", nullable: true),
                    Id_commune = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localite", x => x.Id_localite);
                    table.ForeignKey(
                        name: "FK_Localite_CommuneBXL_Id_commune",
                        column: x => x.Id_commune,
                        principalTable: "CommuneBXL",
                        principalColumn: "Id_commune");
                    table.ForeignKey(
                        name: "FK_Localite_Pays_Id_pays",
                        column: x => x.Id_pays,
                        principalTable: "Pays",
                        principalColumn: "Id_pays");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Donnees_persos_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite");

            migrationBuilder.CreateIndex(
                name: "IX_Localite_Id_commune",
                table: "Localite",
                column: "Id_commune");

            migrationBuilder.CreateIndex(
                name: "IX_Localite_Id_pays",
                table: "Localite",
                column: "Id_pays");

            migrationBuilder.AddForeignKey(
                name: "FK_Donnees_persos_Localite_Id_localite",
                table: "Donnees_persos",
                column: "Id_localite",
                principalTable: "Localite",
                principalColumn: "Id_localite",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donnees_persos_Localite_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.DropTable(
                name: "Localite");

            migrationBuilder.DropTable(
                name: "CommuneBXL");

            migrationBuilder.DropTable(
                name: "Pays");

            migrationBuilder.DropIndex(
                name: "IX_Donnees_persos_Id_localite",
                table: "Donnees_persos");

            migrationBuilder.RenameColumn(
                name: "Id_conducteur",
                table: "Conducteurs",
                newName: "Id");
        }
    }
}
