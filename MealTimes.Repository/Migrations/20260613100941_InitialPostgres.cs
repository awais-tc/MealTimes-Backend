using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessMetrics",
                columns: table => new
                {
                    MetricsID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SubscriptionRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CommissionRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalRevenue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ChefPayouts = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    OperationalCosts = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TotalExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    NetProfit = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ProfitMargin = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m),
                    TotalOrders = table.Column<int>(type: "integer", nullable: false),
                    ActiveSubscriptions = table.Column<int>(type: "integer", nullable: false),
                    ActiveChefs = table.Column<int>(type: "integer", nullable: false),
                    ActiveEmployees = table.Column<int>(type: "integer", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessMetrics", x => x.MetricsID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", precision: 10, scale: 8, nullable: false),
                    Longitude = table.Column<double>(type: "double precision", precision: 11, scale: 8, nullable: false),
                    FormattedAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    SubscriptionPlanID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanName = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MealLimitPerDay = table.Column<int>(type: "integer", nullable: false),
                    DurationInDays = table.Column<int>(type: "integer", nullable: false),
                    IsCustomizable = table.Column<bool>(type: "boolean", nullable: false),
                    MaxEmployees = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.SubscriptionPlanID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminID);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPersons",
                columns: table => new
                {
                    DeliveryPersonID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    VehicleInfo = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPersons", x => x.DeliveryPersonID);
                    table.ForeignKey(
                        name: "FK_DeliveryPersons_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomeChefs",
                columns: table => new
                {
                    ChefID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    LocationID = table.Column<int>(type: "integer", nullable: true),
                    UserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeChefs", x => x.ChefID);
                    table.ForeignKey(
                        name: "FK_HomeChefs_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HomeChefs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CorporateCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: false),
                    AdminID = table.Column<int>(type: "integer", nullable: true),
                    ActiveSubscriptionPlanID = table.Column<int>(type: "integer", nullable: true),
                    PlanStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlanEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LocationID = table.Column<int>(type: "integer", nullable: true),
                    UserID = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionPlanID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporateCompanies", x => x.CompanyID);
                    table.ForeignKey(
                        name: "FK_CorporateCompanies_Admins_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Admins",
                        principalColumn: "AdminID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateCompanies_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CorporateCompanies_SubscriptionPlans_ActiveSubscriptionPlan~",
                        column: x => x.ActiveSubscriptionPlanID,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CorporateCompanies_SubscriptionPlans_SubscriptionPlanID",
                        column: x => x.SubscriptionPlanID,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanID");
                    table.ForeignKey(
                        name: "FK_CorporateCompanies_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChefPayouts",
                columns: table => new
                {
                    PayoutID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChefID = table.Column<int>(type: "integer", nullable: false),
                    TotalEarnings = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CommissionDeducted = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PayoutPeriod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentMethod = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PaymentReference = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
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
                name: "Meals",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChefID = table.Column<int>(type: "integer", nullable: false),
                    MealName = table.Column<string>(type: "text", nullable: false),
                    MealDescription = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MealCategory = table.Column<string>(type: "text", nullable: false),
                    PreparationTime = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Availability = table.Column<bool>(type: "boolean", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.MealID);
                    table.ForeignKey(
                        name: "FK_Meals_HomeChefs_ChefID",
                        column: x => x.ChefID,
                        principalTable: "HomeChefs",
                        principalColumn: "ChefID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySubscriptionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CorporateCompanyId = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionPlanID = table.Column<int>(type: "integer", nullable: false),
                    SubscribedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySubscriptionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySubscriptionHistories_CorporateCompanies_CorporateCo~",
                        column: x => x.CorporateCompanyId,
                        principalTable: "CorporateCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanySubscriptionHistories_SubscriptionPlans_Subscription~",
                        column: x => x.SubscriptionPlanID,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyID = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    DietaryPreferences = table.Column<string>(type: "text", nullable: true),
                    LocationID = table.Column<int>(type: "integer", nullable: true),
                    UserID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_CorporateCompanies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "CorporateCompanies",
                        principalColumn: "CompanyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Employees_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DietaryPreferences",
                columns: table => new
                {
                    DietaryPreferenceID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    Allergies = table.Column<string>(type: "text", nullable: false),
                    Preferences = table.Column<string>(type: "text", nullable: false),
                    Restrictions = table.Column<string>(type: "text", nullable: false),
                    CustomNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietaryPreferences", x => x.DietaryPreferenceID);
                    table.ForeignKey(
                        name: "FK_DietaryPreferences_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    ChefID = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Orders_HomeChefs_ChefID",
                        column: x => x.ChefID,
                        principalTable: "HomeChefs",
                        principalColumn: "ChefID");
                });

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    CommissionID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    ChefID = table.Column<int>(type: "integer", nullable: false),
                    OrderAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ChefPayableAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PlatformEarning = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pending"),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PaymentReference = table.Column<string>(type: "text", nullable: true),
                    ChefPayoutPayoutID = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    DeliveryPersonID = table.Column<int>(type: "integer", nullable: true),
                    DeliveryServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    PickedUpAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryID);
                    table.ForeignKey(
                        name: "FK_Deliveries_DeliveryPersons_DeliveryPersonID",
                        column: x => x.DeliveryPersonID,
                        principalTable: "DeliveryPersons",
                        principalColumn: "DeliveryPersonID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Deliveries_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    EmployeeID = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderMeals",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    MealID = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMeals", x => new { x.OrderID, x.MealID });
                    table.ForeignKey(
                        name: "FK_OrderMeals_Meals_MealID",
                        column: x => x.MealID,
                        principalTable: "Meals",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderMeals_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: true),
                    SubscriptionPlanID = table.Column<int>(type: "integer", nullable: true),
                    CorporateCompanyID = table.Column<int>(type: "integer", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    StripeSessionId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_CorporateCompanies_CorporateCompanyID",
                        column: x => x.CorporateCompanyID,
                        principalTable: "CorporateCompanies",
                        principalColumn: "CompanyID");
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_Payments_SubscriptionPlans_SubscriptionPlanID",
                        column: x => x.SubscriptionPlanID,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "SubscriptionPlanID");
                });

            migrationBuilder.CreateTable(
                name: "ThirdPartyDeliveryServices",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderID = table.Column<int>(type: "integer", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "text", nullable: false),
                    EstimatedDeliveryTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DeliveryPartnerName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdPartyDeliveryServices", x => x.DeliveryID);
                    table.ForeignKey(
                        name: "FK_ThirdPartyDeliveryServices_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserID",
                table: "Admins",
                column: "UserID",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscriptionHistories_CorporateCompanyId",
                table: "CompanySubscriptionHistories",
                column: "CorporateCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubscriptionHistories_SubscriptionPlanID",
                table: "CompanySubscriptionHistories",
                column: "SubscriptionPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_ActiveSubscriptionPlanID",
                table: "CorporateCompanies",
                column: "ActiveSubscriptionPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_AdminID",
                table: "CorporateCompanies",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_LocationID",
                table: "CorporateCompanies",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_SubscriptionPlanID",
                table: "CorporateCompanies",
                column: "SubscriptionPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_CorporateCompanies_UserID",
                table: "CorporateCompanies",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveryPersonID",
                table: "Deliveries",
                column: "DeliveryPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderID",
                table: "Deliveries",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPersons_UserID",
                table: "DeliveryPersons",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DietaryPreferences_EmployeeID",
                table: "DietaryPreferences",
                column: "EmployeeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyID",
                table: "Employees",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LocationID",
                table: "Employees",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserID",
                table: "Employees",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_EmployeeID",
                table: "Feedbacks",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_OrderID",
                table: "Feedbacks",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeChefs_LocationID",
                table: "HomeChefs",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_HomeChefs_UserID",
                table: "HomeChefs",
                column: "UserID",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Meals_ChefID",
                table: "Meals",
                column: "ChefID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderMeals_MealID",
                table: "OrderMeals",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ChefID",
                table: "Orders",
                column: "ChefID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeID",
                table: "Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_Token_IsUsed_ExpiresAt",
                table: "PasswordResetTokens",
                columns: new[] { "Token", "IsUsed", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "PasswordResetTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CorporateCompanyID",
                table: "Payments",
                column: "CorporateCompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SubscriptionPlanID",
                table: "Payments",
                column: "SubscriptionPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdPartyDeliveryServices_OrderID",
                table: "ThirdPartyDeliveryServices",
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
                name: "CompanySubscriptionHistories");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "DietaryPreferences");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "OrderMeals");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ThirdPartyDeliveryServices");

            migrationBuilder.DropTable(
                name: "ChefPayouts");

            migrationBuilder.DropTable(
                name: "DeliveryPersons");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "HomeChefs");

            migrationBuilder.DropTable(
                name: "CorporateCompanies");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
