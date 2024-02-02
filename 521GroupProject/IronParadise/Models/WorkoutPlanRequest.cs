namespace IronParadise.Models
{
    public class WorkoutPlanRequest
    {
        public string gender { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string age { get; set; }
        public string fitnessLevel { get; set; }
        public string days { get; set; }
        public string timePerSession { get; set; }
        public string goal { get; set; }
        public string? UserID { get; set; }
    }
}
