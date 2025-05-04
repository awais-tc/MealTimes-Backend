using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionSupportToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderID",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CorporateCompanyID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeSessionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CorporateCompanyID",
                table: "Payments",
                column: "CorporateCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID",
                unique: true,
                filter: "[OrderID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscriptionPlanID",
                table: "Payments",
                column: "SubscriptionPlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CorporateCompanies_CorporateCompanyID",
                table: "Payments",
                column: "CorporateCompanyID",
                principalTable: "CorporateCompanies",
                principalColumn: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_SubscriptionPlans_SubscriptionPlanID",
                table: "Payments",
                column: "SubscriptionPlanID",
                principalTable: "SubscriptionPlans",
                principalColumn: "SubscriptionPlanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CorporateCompanies_CorporateCompanyID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_SubscriptionPlans_SubscriptionPlanID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CorporateCompanyID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_OrderID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SubscriptionPlanID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CorporateCompanyID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "StripeSessionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanID",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "OrderID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID",
                unique: true);
        }
    }
}
