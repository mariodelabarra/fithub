using Bogus;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.Enums;
using Fithub.Platform.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace Fithub.Platform.API.Tests.Workout.Exercise
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerGetByIdTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<Domain.Workout.Exercise> _exerciseFaker;

        public ExerciseControllerGetByIdTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _exerciseFaker = new Faker<Domain.Workout.Exercise>()
                .RuleFor(prop => prop.Name, faker => faker.Name.JobTitle())
                .RuleFor(prop => prop.Description, faker => faker.Random.String(15))
                .RuleFor(prop => prop.CreatedOn, faker => faker.Date.Recent())
                .RuleFor(prop => prop.Type, faker => faker.PickRandom(ExerciseType.Cardio, ExerciseType.Strength, ExerciseType.Flexibility, ExerciseType.Balance))
                .RuleFor(prop => prop.DifficultyLevel, faker => faker.PickRandom(DifficultyLevel.Beginner, DifficultyLevel.Intermediate, DifficultyLevel.Advanced))
                .RuleFor(prop => prop.Instructions, faker => faker.Lorem.Sentence());
        }

        [Fact]
        public async Task GetById_ReturnsOk()
        {
            //Arrange
            var testExercise = _exerciseFaker.Generate();
            var id = await SeedExercise(testExercise);

            //Act
            var response = await _factory.HttpClient.GetAsync($"/api/exercise/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var exercise = JsonSerializer.Deserialize<Domain.Workout.Exercise>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            exercise.Should().NotBeNull();
            exercise!.Id.Should().Be(id);
            exercise.Name.Should().Be(testExercise.Name);
            exercise.Description.Should().Be(testExercise.Description);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound()
        {
            //Arrange
            var nonExistingId = Guid.NewGuid();

            //Act
            var response = await _factory.HttpClient.GetAsync($"/api/exercise/{nonExistingId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            //Arrange — non-GUID value won't match the {id:guid} route constraint
            var response = await _factory.HttpClient.GetAsync("/api/exercise/not-a-valid-guid");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<Guid> SeedExercise(Domain.Workout.Exercise exercise)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FithubDbContext>();
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();
            return exercise.Id;
        }
    }
}
