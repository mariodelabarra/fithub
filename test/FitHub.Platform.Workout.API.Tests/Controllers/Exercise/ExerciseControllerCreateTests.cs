using Bogus;
using FitHub.Platform.Workout.Domain;
using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerCreateTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<Exercise> _exerciseFaker;
        protected Faker _faker = new();

        public ExerciseControllerCreateTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _exerciseFaker = new Faker<Exercise>()
                .RuleFor(prop => prop.Name, faker => faker.Name.JobTitle())
                .RuleFor(prop => prop.Description, faker => faker.Random.String(15))
                .RuleFor(prop => prop.CreatedOn, faker => faker.Date.Recent())
                .RuleFor(prop => prop.Type, faker => faker.PickRandom<ExerciseType>())
                .RuleFor(prop => prop.DifficultyLevel, faker => faker.PickRandom<DifficultyLevel>())
                .RuleFor(prop => prop.Instructions, faker => faker.Random.String());
        }

        [Fact]
        public async Task Create_ShouldSucceed()
        {
            //Arrange
            Exercise newExercise = _exerciseFaker.Generate();
            var content = new StringContent(JsonSerializer.Serialize(newExercise), encoding: Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PostAsync($"api/exercise/", content);
            var contentResponse = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Example Desc")]
        public async Task Create_ShouldFail_WithInvalid_Input(string description)
        {
            //Arrage
            Exercise newExercise = _exerciseFaker.Generate();
            newExercise.Description = description;
            var content = new StringContent(JsonSerializer.Serialize(newExercise), encoding: Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PostAsync($"api/exercise/", content);

            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
    }
}
