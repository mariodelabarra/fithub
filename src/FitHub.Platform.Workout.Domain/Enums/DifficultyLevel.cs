using System.Text.Json.Serialization;

namespace FitHub.Platform.Workout.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DifficultyLevel
    {
        None,
        Beginner,
        Intermediate,
        Advanced
    }
}
