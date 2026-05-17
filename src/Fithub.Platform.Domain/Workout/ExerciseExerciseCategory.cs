namespace Fithub.Platform.Domain.Workout;

public class ExerciseExerciseCategory
{
    public Guid ExerciseId { get; set; }
    public Guid ExerciseCategoryId { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public ExerciseCategory ExerciseCategory { get; set; } = null!;
}
