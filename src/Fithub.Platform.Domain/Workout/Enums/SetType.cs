using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SetType
{
    Standard = 1,
    Warmup = 2,
    DropSet = 3,
    FailureSet = 4
}
