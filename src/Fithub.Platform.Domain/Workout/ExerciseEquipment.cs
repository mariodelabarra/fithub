namespace Fithub.Platform.Domain.Workout;

public class ExerciseEquipment
{
    public Guid ExerciseId { get; set; }
    public Guid EquipmentId { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public Equipment Equipment { get; set; } = null!;
}
