using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExerciseType
{
    Strength = 1,
    Cardio = 2,
    Flexibility = 3,
    Balance = 4,
    Plyometric = 5,
    Calisthenics = 6
}
