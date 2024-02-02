using System.ComponentModel.DataAnnotations.Schema;

namespace IronParadise.Models
{
    public class Exercise
    {
        public Guid Id { get; set; }

        //Stored as "Name"
        public string Name { get; set; }

        //Number of times you do the workout
        public int Sets { get; set; }

        //Number of iterations in set
        public int Reps { get; set; }

        [ForeignKey(nameof(Workout.Id))]
        public Guid WorkoutID { get; set; }
    }
}
