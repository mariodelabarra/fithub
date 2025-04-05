using System.Text.Json.Serialization;

namespace FitHub.Platform.Workout.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced
    }
}
