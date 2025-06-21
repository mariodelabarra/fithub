using Bogus;
using Dapper;
using FitHub.Platform.Workout.Domain;
using MySql.Data.MySqlClient;
using System.Text;
using System.Text.Json;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerUpdateTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<Exercise> _exerciseFaker;
        protected Faker _faker = new();

        public ExerciseControllerUpdateTests(CustomWebApplicationFactory factory)
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
        public async Task Update_ShouldSucceed()
        {
            //Arrage
            Exercise newExercise = _exerciseFaker.Generate();
            var id = await CreatingExercise(newExercise);
            var content = new StringContent(JsonSerializer.Serialize(newExercise), encoding: Encoding.UTF8, "application/json");

            //Act
            var response = await _factory.HttpClient.PutAsync($"api/exercise/{id}", content);
            var contenta = await response.Content.ReadAsStringAsync();

            //Act
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

        private async Task<int> CreatingExercise(Exercise exercise)
        {
            await using var connection = new MySqlConnection(_factory.GetConnectionString());
            await connection.OpenAsync();

            var sql = @"
                    INSERT INTO Exercises (Name, Description, CreatedOn)
                    VALUES (@Name, @Description, @CreatedOn);
                    SELECT LAST_INSERT_ID();";

            var id = await connection.ExecuteScalarAsync<int>(sql, new
            {
                exercise.Name,
                exercise.Description,
                exercise.CreatedOn
            });

            return id;
        }
    }
}
