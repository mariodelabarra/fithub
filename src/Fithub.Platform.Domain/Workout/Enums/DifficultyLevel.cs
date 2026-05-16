using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DifficultyLevel
{
    None,
    Beginner,
    Intermediate,
    Advanced
}
