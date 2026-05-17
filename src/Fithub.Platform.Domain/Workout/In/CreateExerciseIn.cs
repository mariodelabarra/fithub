using Fithub.Platform.Domain.Workout.Enums;
using FluentValidation;

namespace Fithub.Platform.Domain.Workout.In;

public record CreateExerciseIn
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseType Type { get; set; } = ExerciseType.Strength;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
    public string Instructions { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public bool IsPublic { get; set; } = true;
    public List<MuscleGroupIn> MuscleGroups { get; set; } = [];
    public List<Guid> CategoryIds { get; set; } = [];
    public List<Guid> EquipmentIds { get; set; } = [];
}

public record MuscleGroupIn
{
    public MuscleGroup MuscleGroup { get; set; }
    public bool IsPrimary { get; set; } = true;
}

public class CreateExerciseInValidator : AbstractValidator<CreateExerciseIn>
{
    public CreateExerciseInValidator()
    {
        RuleFor(e => e.Name).NotEmpty();

        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(15);

        RuleFor(e => e.Type).IsInEnum();
        RuleFor(e => e.DifficultyLevel).IsInEnum();
    }
}
