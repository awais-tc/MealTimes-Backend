using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTimes.Core.Models
{
        public class User
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int UserID { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string PasswordHash { get; set; }

            [Required]
            public string Role { get; set; }

            public virtual HomeChef? HomeChef { get; set; }
            public virtual CorporateCompany? CorporateCompany { get; set; }
            public virtual Employee? Employee { get; set; }
            public virtual Admin? Admin { get; set; }
            public virtual DeliveryPerson? DeliveryPerson { get; set; }
        }
}
