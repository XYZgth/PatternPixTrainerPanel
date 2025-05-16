using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatternPixTrainerPanel.Migrations
{
    /**
     * \brief Initiale Migration zum Erstellen der Datenbanktabellen für Children und Trainings.
     */
    public partial class InitialMigration : Migration
    {
        /**
         * \brief Führt die Migration durch und erstellt die Tabellen.
         * 
         * Erstellt die Tabellen "Children" und "Trainings" mit ihren jeweiligen Spalten und Beziehungen.
         * 
         * \param migrationBuilder Der MigrationBuilder zum Aufbau der Datenbankstruktur.
         */
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Children",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Children", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChildId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimeOfDay = table.Column<long>(type: "INTEGER", nullable: false),
                    Symmetry = table.Column<char>(type: "TEXT", nullable: false),
                    Errors = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeNeeded = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainings_Children_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_ChildId",
                table: "Trainings",
                column: "ChildId");
        }

        /**
         * \brief Macht die Migration rückgängig, entfernt die erstellten Tabellen.
         * 
         * Entfernt die Tabellen "Trainings" und "Children" aus der Datenbank.
         * 
         * \param migrationBuilder Der MigrationBuilder zur Rückgängigmachung der Datenbankänderungen.
         */
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "Children");
        }
    }
}
