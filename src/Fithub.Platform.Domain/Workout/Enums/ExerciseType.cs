using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExerciseType
{
    None,
    Cardio,
    Strength,
    Flexibility,
    Balance
}
