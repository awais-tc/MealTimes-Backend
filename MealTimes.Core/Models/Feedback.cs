using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackID { get; set; }

        public int OrderID { get; set; }
        public int EmployeeID { get; set; }

        [Required]
        public int Rating { get; set; }

        public string Comments { get; set; }

        public Order Order { get; set; }
        public Employee Employee { get; set; }
    }
}
