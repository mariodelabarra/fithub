using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class SessionExerciseLog : BaseEntity
{
    public Guid WorkoutSessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public Guid? WorkoutExerciseId { get; set; }
    public int Order { get; set; }
    public string? Notes { get; set; }

    public WorkoutSession WorkoutSession { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
    public WorkoutExercise? WorkoutExercise { get; set; }
    public ICollection<SessionSetLog> SetLogs { get; set; } = [];
}
