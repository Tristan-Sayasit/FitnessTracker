namespace FitnessTracker.Models
{
    public class SplitDaysWorkoutsModel
    {
        public string? split_day_workout_id { get; set; }
        public string? split_day_id { get; set; }
        public string? workout_type_id { get; set; }
        public int sets { get; set; }
        public int reps { get; set; }
        public float weight { get; set; }
    }
}
