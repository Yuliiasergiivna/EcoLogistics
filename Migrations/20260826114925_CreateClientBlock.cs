using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoLogistics.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiegeSociales",
                columns: table => new
                {
                    Id_siege = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Raison_sociale = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Site_internet = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Secteur_activite = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_localite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiegeSociales", x => x.Id_siege);
                    table.ForeignKey(
                        name: "FK_SiegeSociales_Localites_Id_localite",
                        column: x => x.Id_localite,
                        principalTable: "Localites",
                        principalColumn: "Id_localite");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id_client = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nom_entreprise = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero_entreprise = table.Column<int>(type: "int", nullable: true),
                    BE_entreprise = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remarques = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enregistrement_BE = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Agrement_BE = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type_enregistrement = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Presentation = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Id_localite = table.Column<int>(type: "int", nullable: true),
                    Id_user = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Id_siege = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id_client);
                    table.ForeignKey(
                        name: "FK_Clients_Localites_Id_localite",
                        column: x => x.Id_localite,
                        principalTable: "Localites",
                        principalColumn: "Id_localite");
                    table.ForeignKey(
                        name: "FK_Clients_SiegeSociales_Id_siege",
                        column: x => x.Id_siege,
                        principalTable: "SiegeSociales",
                        principalColumn: "Id_siege");
                    table.ForeignKey(
                        name: "FK_Clients_users_Id_user",
                        column: x => x.Id_user,
                        principalTable: "users",
                        principalColumn: "Id_user");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AdressesExploitation",
                columns: table => new
                {
                    Id_adresse_exp = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_site = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rue = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Numero = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_client = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Id_localite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdressesExploitation", x => x.Id_adresse_exp);
                    table.ForeignKey(
                        name: "FK_AdressesExploitation_Clients_Id_client",
                        column: x => x.Id_client,
                        principalTable: "Clients",
                        principalColumn: "Id_client",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdressesExploitation_Localites_Id_localite",
                        column: x => x.Id_localite,
                        principalTable: "Localites",
                        principalColumn: "Id_localite");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PersonneContacts",
                columns: table => new
                {
                    Id_contact = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gsm = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Id_client = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Id_localite = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonneContacts", x => x.Id_contact);
                    table.ForeignKey(
                        name: "FK_PersonneContacts_Clients_Id_client",
                        column: x => x.Id_client,
                        principalTable: "Clients",
                        principalColumn: "Id_client",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonneContacts_Localites_Id_localite",
                        column: x => x.Id_localite,
                        principalTable: "Localites",
                        principalColumn: "Id_localite");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AdressesExploitation_Id_client",
                table: "AdressesExploitation",
                column: "Id_client");

            migrationBuilder.CreateIndex(
                name: "IX_AdressesExploitation_Id_localite",
                table: "AdressesExploitation",
                column: "Id_localite");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Id_localite",
                table: "Clients",
                column: "Id_localite");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Id_siege",
                table: "Clients",
                column: "Id_siege");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Id_user",
                table: "Clients",
                column: "Id_user");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneContacts_Id_client",
                table: "PersonneContacts",
                column: "Id_client");

            migrationBuilder.CreateIndex(
                name: "IX_PersonneContacts_Id_localite",
                table: "PersonneContacts",
                column: "Id_localite");

            migrationBuilder.CreateIndex(
                name: "IX_SiegeSociales_Id_localite",
                table: "SiegeSociales",
                column: "Id_localite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdressesExploitation");

            migrationBuilder.DropTable(
                name: "PersonneContacts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "SiegeSociales");
        }
    }
}
