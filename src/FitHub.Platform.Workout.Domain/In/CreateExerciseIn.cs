using FluentValidation;

namespace FitHub.Platform.Workout.Domain
{
    public record CreateExerciseIn
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ExerciseType Type { get; set; } = ExerciseType.None;
        public MuscleGroup[] MuscleGroups { get; set; } = Array.Empty<MuscleGroup>();
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
    }

    public class CreateExerciseInValidator : AbstractValidator<CreateExerciseIn>
    {
        public CreateExerciseInValidator()
        {
            RuleFor(exercise => exercise.Name).NotEmpty();

            RuleFor(exercise => exercise.Description)
                .NotEmpty()
                .MinimumLength(15);
        }
    }
}
