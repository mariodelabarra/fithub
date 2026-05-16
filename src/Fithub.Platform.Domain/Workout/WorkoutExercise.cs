using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class WorkoutExercise : BaseEntity
{
    public int WorkoutDayId { get; set; }
    public int ExerciseId { get; set; }
    public int[] Sets { get; set; } = [];
    public int[] Reps { get; set; } = [];
    public int RestDuration { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int Order { get; set; }
}
