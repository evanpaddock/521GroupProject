using System.ComponentModel.DataAnnotations.Schema;

namespace IronParadise.Models
{
    public class Workout
    {
        public Guid Id { get; set; }

        public string Day { get; set; }
        public List<Exercise> Exercises { get; set; }

        [ForeignKey(nameof(User.Id))]
        public Guid UserId { get; set; }
    }
}
