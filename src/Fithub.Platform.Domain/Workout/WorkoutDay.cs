using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutDay : BaseEntity
{
    public Guid WorkoutTemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? EstimatedDurationMinutes { get; set; }
    public string? Notes { get; set; }

    public WorkoutTemplate WorkoutTemplate { get; set; } = null!;
    public ICollection<WorkoutExercise> Exercises { get; set; } = [];
    public ICollection<WorkoutSession> Sessions { get; set; } = [];
}
