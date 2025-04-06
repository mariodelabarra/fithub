using System.Text.Json.Serialization;

namespace FitHub.Platform.Workout.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExerciseType
    {
        None,
        Cardio,
        Strength,
        Flexibility,
        Balance
    }
}
