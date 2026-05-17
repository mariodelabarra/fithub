using Fithub.Platform.Domain.Workout.Enums;
using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutTemplate : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
    public bool IsPublic { get; set; } = false;
    public int? EstimatedDurationMinutes { get; set; }

    public ICollection<WorkoutDay> Days { get; set; } = [];
}
