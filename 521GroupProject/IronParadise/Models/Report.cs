namespace IronParadise.Models
{
    public class MostTypeOfUsersReport
    {
        public string? FitnessLevel { get; set; }
        public int? Frequency { get; set; }
    }

    public class AverageTimePerSessionReport
    {
        public double? AverageDuration { get; set; }
    }

    public class MostCommonGoalReport
    {
        public string? WorkoutGoal { get; set; }
        public int? Frequency { get; set; }
    }

    public class NumberOfUsersReport
    {
        public int? UserCount { get; set; }
    }

}
