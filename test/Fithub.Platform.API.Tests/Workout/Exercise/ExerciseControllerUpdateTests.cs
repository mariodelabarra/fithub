using Bogus;
using Fithub.Platform.Domain.Workout;
using Fithub.Platform.Domain.Workout.Enums;
using Fithub.Platform.Domain.Workout.In;
using Fithub.Platform.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace Fithub.Platform.API.Tests.Workout.Exercise
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerUpdateTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<Fithub.Platform.Domain.Workout.Exercise> _exerciseFaker;
        protected readonly Faker _faker = new();

        public ExerciseControllerUpdateTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _exerciseFaker = new Faker<Fithub.Platform.Domain.Workout.Exercise>()
                .RuleFor(prop => prop.Name, faker => faker.Name.JobTitle())
                .RuleFor(prop => prop.Description, faker => faker.Random.String(15))
                .RuleFor(prop => prop.CreatedOn, faker => faker.Date.Recent())
                .RuleFor(prop => prop.Type, faker => faker.PickRandom(ExerciseType.Cardio, ExerciseType.Strength, ExerciseType.Flexibility, ExerciseType.Balance))
                .RuleFor(prop => prop.DifficultyLevel, faker => faker.PickRandom(DifficultyLevel.Beginner, DifficultyLevel.Intermediate, DifficultyLevel.Advanced))
                .RuleFor(prop => prop.Instructions, faker => faker.Lorem.Sentence());
        }

        [Fact]
        public async Task Update_ShouldSucceed()
        {
            //Arrange
            var exercise = _exerciseFaker.Generate();
            var id = await SeedExercise(exercise);

            var updatePayload = new UpdateExerciseIn
            {
                Description = _faker.Random.String(15),
                Type = ExerciseType.Cardio,
                Instructions = _faker.Lorem.Sentence()
            };
            var content = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PutAsync($"api/exercise/{id}", content);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update_ShouldFail_WithInvalid_Input()
        {
        }

        [Fact]
        public async Task Update_ShouldFail_WithNoExistingEntity()
        {
        }

        private async Task<Guid> SeedExercise(Fithub.Platform.Domain.Workout.Exercise exercise)
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FithubDbContext>();
            context.Exercises.Add(exercise);
            await context.SaveChangesAsync();
            return exercise.Id;
        }
    }
}
