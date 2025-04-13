using Bogus;

namespace FitHub.Platform.Workout.Domain.Tests
{
    public class ExerciseFaker : Faker<Exercise>
    {
        public ExerciseFaker()
        {
            RuleFor(exercise => exercise.Name, faker => faker.Random.String());
            RuleFor(exercise => exercise.Description, faker => faker.Random.String());
            RuleFor(exercise => exercise.Type, faker => faker.PickRandom<ExerciseType>());
        }
    }
}
