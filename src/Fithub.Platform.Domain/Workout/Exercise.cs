using Fithub.Platform.Domain.Workout.Enums;
using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ExerciseType Type { get; set; } = ExerciseType.Strength;
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
    public string Instructions { get; set; } = string.Empty;
    public string? VideoUrl { get; set; }
    public bool IsPublic { get; set; } = true;
    public string? CreatedByUserId { get; set; }

    public ICollection<ExerciseMuscleGroup> MuscleGroups { get; set; } = [];
    public ICollection<ExerciseExerciseCategory> Categories { get; set; } = [];
    public ICollection<ExerciseEquipment> Equipment { get; set; } = [];
}
