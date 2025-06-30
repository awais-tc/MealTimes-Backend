using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTimes.Core.DTOs
{
    public class DietaryPreferenceDto
    {
        public List<string> Allergies { get; set; } = new();
        public List<string> Preferences { get; set; } = new();
        public List<string> Restrictions { get; set; } = new();
        public string? CustomNotes { get; set; }
    }
}
