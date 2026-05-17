using Fithub.Platform.Domain.Workout.Enums;
using FluentValidation;

namespace Fithub.Platform.Domain.Workout.In;

public record UpdateExerciseIn
{
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

public class UpdateExerciseInValidator : AbstractValidator<UpdateExerciseIn>
{
    public UpdateExerciseInValidator()
    {
        RuleFor(e => e.Description)
            .NotEmpty()
            .MinimumLength(15);

        RuleFor(e => e.Type).IsInEnum();
        RuleFor(e => e.DifficultyLevel).IsInEnum();
    }
}
