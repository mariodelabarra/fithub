using FitHub.Platform.Common.Domain;

namespace FitHub.Platform.Workout.Domain
{
    public class WorkoutExercise : BaseEntity
    {
        public int WorkoutDayId { get; set; }
        public int ExerciseId { get; set; }

        public int[] Sets { get; set; } = Array.Empty<int>();
        public int[] Reps { get; set; } = Array.Empty<int>();

        public int RestDuration { get; set; }
        public string Notes { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}
