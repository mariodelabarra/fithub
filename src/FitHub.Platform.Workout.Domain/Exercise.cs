using FitHub.Platform.Common.Domain;

namespace FitHub.Platform.Workout.Domain
{
    public class Exercise : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ExerciseType Type { get; set; } = ExerciseType.None;
        public MuscleGroup[] MuscleGroups { get; set; } = Array.Empty<MuscleGroup>();
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
        public int[] Sets { get; set; } = Array.Empty<int>();
        public int[] Reps { get; set; } = Array.Empty<int>();
        //Image-Video prop
        public string Instructions { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public int DurationOfRest { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
