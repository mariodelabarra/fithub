using Fithub.Platform.Domain.Workout.Enums;
using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutExerciseSet : BaseEntity
{
    public Guid WorkoutExerciseId { get; set; }
    public int SetNumber { get; set; }
    public SetType SetType { get; set; } = SetType.Standard;

    // Strength / calisthenics
    public short? TargetReps { get; set; }
    public decimal? TargetWeightKg { get; set; }

    // Cardio / timed
    public int? TargetDurationSeconds { get; set; }
    public decimal? TargetDistanceMeters { get; set; }

    public byte? TargetRpe { get; set; }

    public WorkoutExercise WorkoutExercise { get; set; } = null!;
}
