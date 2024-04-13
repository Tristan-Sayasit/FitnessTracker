namespace FitnessTracker.Models
{
    public class SplitDaysModel
    {
        public string? split_day_id { get; set; }
        public DateOnly day_of_week { get; set; }
        public string? split_id { get; set; }
    }
}
