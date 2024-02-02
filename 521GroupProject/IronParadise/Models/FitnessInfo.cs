using System.ComponentModel.DataAnnotations.Schema;

namespace IronParadise.Models
{
    public class FitnessInfo
    {
        public Guid Id { get; set; }
        public int HeightInInches { get; set; }
        public string Age { get; set; }

        //Male, Female
        public string SexAtBirth { get; set; }
        public float Weight { get; set; }

        //beginner, intermediate, pro
        public string FitnessLevel { get; set; }

        //Delemited by commas, "Mon,Tue,Wed,Thu,Fri,Sat,Sun"
        public string PreferredDays { get; set;}
        public int SelectedDurationInMinutes { get; set; }

        //Lose fat, gain muscle,stay in shape
        public string WorkoutGoal { get; set; }

        [ForeignKey(nameof(User.Id))]
        public string UserId { get; set; }
    }
}
