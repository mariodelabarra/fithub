using Fithub.Platform.Domain.Workout.Enums;
using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class SessionSetLog : BaseEntity
{
    public Guid SessionExerciseLogId { get; set; }
    public int SetNumber { get; set; }
    public SetType SetType { get; set; } = SetType.Standard;

    public short? ActualReps { get; set; }
    public decimal? ActualWeightKg { get; set; }
    public int? ActualDurationSeconds { get; set; }
    public decimal? ActualDistanceMeters { get; set; }

    public byte? Rpe { get; set; }
    public bool IsCompleted { get; set; } = true;

    public SessionExerciseLog SessionExerciseLog { get; set; } = null!;
}
