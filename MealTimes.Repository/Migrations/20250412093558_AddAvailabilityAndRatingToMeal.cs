using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealTimes.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailabilityAndRatingToMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Meals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Meals",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Meals");
        }
    }
}
