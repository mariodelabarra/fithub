using FluentValidation;

namespace FitHub.Platform.Workout.Domain
{
    public class UpdateExerciseIn
    {
        public string Description { get; set; } = string.Empty;
        public ExerciseType Type { get; set; } = ExerciseType.None;
        public MuscleGroup[] MuscleGroups { get; set; } = Array.Empty<MuscleGroup>();
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
        public int[] Sets { get; set; } = Array.Empty<int>();
        public int[] Reps { get; set; } = Array.Empty<int>();
        public string Instructions { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public int DurationOfRest { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateExerciseInValidator : AbstractValidator<UpdateExerciseIn>
    {
        public UpdateExerciseInValidator()
        {   
            RuleFor(exercise => exercise.Description)
                .NotEmpty()
                .MinimumLength(15);

            RuleFor(exercise => exercise.Type)
                .NotEqual(ExerciseType.None);

            RuleFor(exercise => exercise.MuscleGroups)
                .NotEmpty();

            RuleFor(exercise => exercise.Sets)
                .NotEmpty();

            RuleFor(exercise => exercise.Reps)
                .NotEmpty();

            RuleFor(exercise => exercise.DurationOfRest)
                .GreaterThan(0);
        }
    }
}
