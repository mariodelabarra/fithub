using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutExercise : BaseEntity
{
    public Guid WorkoutDayId { get; set; }
    public Guid ExerciseId { get; set; }
    public int Order { get; set; }
    public string? Notes { get; set; }
    public short? RestDurationSeconds { get; set; }

    public WorkoutDay WorkoutDay { get; set; } = null!;
    public Exercise Exercise { get; set; } = null!;
    public ICollection<WorkoutExerciseSet> Sets { get; set; } = [];
}
