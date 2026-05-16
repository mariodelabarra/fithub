using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MuscleGroup
{
    Chest,
    Legs,
    Core
}
