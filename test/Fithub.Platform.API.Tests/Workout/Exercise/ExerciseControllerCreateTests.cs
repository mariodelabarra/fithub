using Bogus;
using Fithub.Platform.Domain.Workout.Enums;
using Fithub.Platform.Domain.Workout.In;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Fithub.Platform.API.Tests.Workout.Exercise
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerCreateTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<CreateExerciseIn> _exerciseFaker;

        public ExerciseControllerCreateTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _exerciseFaker = new Faker<CreateExerciseIn>()
                .RuleFor(prop => prop.Name, faker => faker.Name.JobTitle())
                .RuleFor(prop => prop.Description, faker => faker.Random.String(15))
                .RuleFor(prop => prop.Type, faker => faker.PickRandom(ExerciseType.Cardio, ExerciseType.Strength, ExerciseType.Flexibility, ExerciseType.Balance))
                .RuleFor(prop => prop.DifficultyLevel, faker => faker.PickRandom(DifficultyLevel.Beginner, DifficultyLevel.Intermediate, DifficultyLevel.Advanced));
        }

        [Fact]
        public async Task Create_ShouldSucceed()
        {
            //Arrange
            var newExercise = _exerciseFaker.Generate();
            var content = new StringContent(JsonSerializer.Serialize(newExercise), Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PostAsync("api/exercise/", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Example Desc")]
        public async Task Create_ShouldFail_WithInvalid_Input(string description)
        {
            //Arrange
            var newExercise = _exerciseFaker.Generate();
            newExercise.Description = description;
            var content = new StringContent(JsonSerializer.Serialize(newExercise), Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PostAsync("api/exercise/", content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
    }
}
