using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutSession : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid? WorkoutDayId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }

    public WorkoutDay? WorkoutDay { get; set; }
    public ICollection<SessionExerciseLog> ExerciseLogs { get; set; } = [];
}
