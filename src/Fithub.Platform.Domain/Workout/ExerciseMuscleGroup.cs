using Fithub.Platform.Domain.Workout.Enums;

namespace Fithub.Platform.Domain.Workout;

public class ExerciseMuscleGroup
{
    public Guid ExerciseId { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public bool IsPrimary { get; set; } = true;

    public Exercise Exercise { get; set; } = null!;
}
