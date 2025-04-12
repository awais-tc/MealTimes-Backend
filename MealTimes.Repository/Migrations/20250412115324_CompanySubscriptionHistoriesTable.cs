using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CompanySubscriptionHistoriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanySubscriptionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorporateCompanyId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionPlanID = table.Column<int>(type: "int", nullable: false),
                    SubscribedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySubscriptionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySubscriptionHistories_CorporateCompanies_CorporateCompanyId",
                        column: x => x.CorporateCompanyId,
                        principalTable: "CorporateCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanySubscriptionHistories_SubscriptionPlans_SubscriptionPlanID",
                        column: x => x.SubscriptionPlanID,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscriptionHistories_CorporateCompanyId",
                table: "CompanySubscriptionHistories",
                column: "CorporateCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscriptionHistories_SubscriptionPlanID",
                table: "CompanySubscriptionHistories",
                column: "SubscriptionPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySubscriptionHistories");
        }
    }
}
