namespace MealTimes.Core.Models
{
    public class DietaryPreference
    {
        public int DietaryPreferenceID { get; set; }

        public int EmployeeID { get; set; }
        public List<string> Allergies { get; set; } = new();
        public List<string> Preferences { get; set; } = new();
        public List<string> Restrictions { get; set; } = new();
        public string? CustomNotes { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
