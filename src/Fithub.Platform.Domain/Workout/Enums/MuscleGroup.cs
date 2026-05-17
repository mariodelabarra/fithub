using System.Text.Json.Serialization;

namespace Fithub.Platform.Domain.Workout.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MuscleGroup
{
    Chest = 1,
    Back = 2,
    Shoulders = 3,
    Biceps = 4,
    Triceps = 5,
    Forearms = 6,
    Core = 7,
    Glutes = 8,
    Quadriceps = 9,
    Hamstrings = 10,
    Calves = 11,
    HipFlexors = 12
}
