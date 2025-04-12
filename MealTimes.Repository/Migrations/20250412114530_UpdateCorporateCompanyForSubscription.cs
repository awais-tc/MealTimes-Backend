using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCorporateCompanyForSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_SubscriptionPlanID",
                table: "CorporateCompanies");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "SubscriptionPlans",
                newName: "MaxEmployees");

            migrationBuilder.AddColumn<int>(
                name: "DurationInDays",
                table: "SubscriptionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomizable",
                table: "SubscriptionPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionPlanID",
                table: "CorporateCompanies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ActiveSubscriptionPlanID",
                table: "CorporateCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanEndDate",
                table: "CorporateCompanies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanStartDate",
                table: "CorporateCompanies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_ActiveSubscriptionPlanID",
                table: "CorporateCompanies",
                column: "ActiveSubscriptionPlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_ActiveSubscriptionPlanID",
                table: "CorporateCompanies",
                column: "ActiveSubscriptionPlanID",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_SubscriptionPlanID",
                table: "CorporateCompanies",
                column: "SubscriptionPlanID",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_ActiveSubscriptionPlanID",
                table: "CorporateCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_SubscriptionPlanID",
                table: "CorporateCompanies");

            migrationBuilder.DropIndex(
                name: "IX_CorporateCompanies_ActiveSubscriptionPlanID",
                table: "CorporateCompanies");

            migrationBuilder.DropColumn(
                name: "DurationInDays",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "IsCustomizable",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "ActiveSubscriptionPlanID",
                table: "CorporateCompanies");

            migrationBuilder.DropColumn(
                name: "PlanEndDate",
                table: "CorporateCompanies");

            migrationBuilder.DropColumn(
                name: "PlanStartDate",
                table: "CorporateCompanies");

            migrationBuilder.RenameColumn(
                name: "MaxEmployees",
                table: "SubscriptionPlans",
                newName: "Duration");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionPlanID",
                table: "CorporateCompanies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CorporateCompanies_SubscriptionPlans_SubscriptionPlanID",
                table: "CorporateCompanies",
                column: "SubscriptionPlanID",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
