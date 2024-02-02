namespace IronParadise.Models
{
    public class WorkoutPlan
        {
        public string DayName { get; set; }
        public List<Tuple<string, int, int>> Exercises { get; set; }
    }
}
