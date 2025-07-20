using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "HomeChefs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationID",
                table: "CorporateCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float(10)", precision: 10, scale: 8, nullable: false),
                    Longitude = table.Column<double>(type: "float(11)", precision: 11, scale: 8, nullable: false),
                    FormattedAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeChefs_LocationID",
                table: "HomeChefs",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LocationID",
                table: "Employees",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_LocationID",
                table: "CorporateCompanies",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_City",
                table: "Locations",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Latitude_Longitude",
                table: "Locations",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_State",
                table: "Locations",
                column: "State");

            migrationBuilder.AddForeignKey(
                name: "FK_CorporateCompanies_Locations_LocationID",
                table: "CorporateCompanies",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Locations_LocationID",
                table: "Employees",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_HomeChefs_Locations_LocationID",
                table: "HomeChefs",
                column: "LocationID",
                principalTable: "Locations",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorporateCompanies_Locations_LocationID",
                table: "CorporateCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Locations_LocationID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeChefs_Locations_LocationID",
                table: "HomeChefs");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_HomeChefs_LocationID",
                table: "HomeChefs");

            migrationBuilder.DropIndex(
                name: "IX_Employees_LocationID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CorporateCompanies_LocationID",
                table: "CorporateCompanies");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "HomeChefs");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "CorporateCompanies");
        }
    }
}
