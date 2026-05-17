using FitHub.Platform.Common.Domain;

namespace Fithub.Platform.Domain.Workout;

public class Equipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<ExerciseEquipment> Exercises { get; set; } = [];
}
