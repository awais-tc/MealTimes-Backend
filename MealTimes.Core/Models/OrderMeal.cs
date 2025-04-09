

namespace MealTimes.Core.Models
{
    public class OrderMeal
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int MealID { get; set; }
        public Meal Meal { get; set; }

        public int Quantity { get; set; }  // Number of times this meal is in the order
    }

}
