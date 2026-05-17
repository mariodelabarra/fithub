using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class ExerciseCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<ExerciseExerciseCategory> Exercises { get; set; } = [];
}
