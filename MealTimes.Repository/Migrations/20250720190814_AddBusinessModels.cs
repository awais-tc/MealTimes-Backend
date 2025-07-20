using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessMetrics",
                columns: table => new
                {
                    MetricsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CommissionRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ChefPayouts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    OperationalCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    NetProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ProfitMargin = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    ActiveSubscriptions = table.Column<int>(type: "int", nullable: false),
                    ActiveChefs = table.Column<int>(type: "int", nullable: false),
                    ActiveEmployees = table.Column<int>(type: "int", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessMetrics", x => x.MetricsID);
                });

            migrationBuilder.CreateTable(
                name: "ChefPayouts",
                columns: table => new
                {
                    PayoutID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChefID = table.Column<int>(type: "int", nullable: false),
                    TotalEarnings = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionDeducted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayoutPeriod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefPayouts", x => x.PayoutID);
                    table.ForeignKey(
                        name: "FK_ChefPayouts_HomeChefs_ChefID",
                        column: x => x.ChefID,
                        principalTable: "HomeChefs",
                        principalColumn: "ChefID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    CommissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ChefID = table.Column<int>(type: "int", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChefPayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlatformEarning = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChefPayoutPayoutID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.CommissionID);
                    table.ForeignKey(
                        name: "FK_Commissions_ChefPayouts_ChefPayoutPayoutID",
                        column: x => x.ChefPayoutPayoutID,
                        principalTable: "ChefPayouts",
                        principalColumn: "PayoutID");
                    table.ForeignKey(
                        name: "FK_Commissions_HomeChefs_ChefID",
                        column: x => x.ChefID,
                        principalTable: "HomeChefs",
                        principalColumn: "ChefID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessMetrics_Date",
                table: "BusinessMetrics",
                column: "Date",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChefPayouts_ChefID_PeriodStart_PeriodEnd",
                table: "ChefPayouts",
                columns: new[] { "ChefID", "PeriodStart", "PeriodEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_ChefPayouts_Status",
                table: "ChefPayouts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ChefID_CreatedAt",
                table: "Commissions",
                columns: new[] { "ChefID", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ChefPayoutPayoutID",
                table: "Commissions",
                column: "ChefPayoutPayoutID");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_OrderID",
                table: "Commissions",
                column: "OrderID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessMetrics");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.DropTable(
                name: "ChefPayouts");
        }
    }
}
