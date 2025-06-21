using Bogus;
using Dapper;
using FitHub.Platform.Workout.Domain;
using FluentAssertions;
using MySql.Data.MySqlClient;
using System.Net;
using System.Text.Json;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    [Collection("ExerciseTests")]
    public class ExerciseControllerGetByIdTests
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly Faker<Exercise> _exerciseFaker;
        protected Faker _faker = new();

        public ExerciseControllerGetByIdTests(CustomWebApplicationFactory factory)
        {
            _factory  = factory;
            _exerciseFaker = new Faker<Exercise>()
                .RuleFor(prop => prop.Name, faker => faker.Name.JobTitle())
                .RuleFor(prop => prop.Description, faker => faker.Random.String(15))
                .RuleFor(prop => prop.CreatedOn, faker => faker.Date.Recent())
                .RuleFor(prop => prop.Type, faker => faker.PickRandom<ExerciseType>())
                .RuleFor(prop => prop.DifficultyLevel, faker => faker.PickRandom<DifficultyLevel>())
                .RuleFor(prop => prop.Instructions, faker => faker.Random.String());
        }
        [Fact]
        public async Task GetById_ReturnsOk()
        {
            //Arrange
            Exercise testExercise = _exerciseFaker.Generate();
            testExercise.Id = await SeedExercise(testExercise);

            //Act
            var response = await _factory.HttpClient.GetAsync($"/api/exercise/{testExercise.Id}");
            var content = await response.Content.ReadAsStringAsync();
            var exercise = JsonSerializer.Deserialize<Exercise>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            exercise.Should().NotBeNull();
            exercise.Id.Should().Be(testExercise.Id);
            exercise.Name.Should().Be(testExercise.Name);
            exercise.Description.Should().Be(testExercise.Description);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound()
        {
            //Arrange
            int nonExistingId = 99999;

            //Act
            var response = await _factory.HttpClient.GetAsync($"/api/exercise/{nonExistingId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsBadRequest()
        {
            //Arrange
            const int invalidId = -1;

            //Act
            var response = await _factory.HttpClient.GetAsync($"/api/exercise/{invalidId}");
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private async Task<string> SeedExercise(Exercise exercise)
        {
            var maxRetries = 3;
            var currentTry = 0;

            while (currentTry < maxRetries)
            {
                try
                {
                    await using var connection = new MySqlConnection(_factory.GetConnectionString());
                    await connection.OpenAsync();

                    const string sql = @"
                    INSERT INTO Exercises(Name, Description, CreatedOn, Type, DifficultyLevel, Instructions)
                    VALUES (@Name, @Description, @CreatedOn, @Type, @DifficultyLevel, @Instructions)";

                    await connection.ExecuteAsync(sql, exercise);

                    const string getIdSql = "SELECT LAST_INSERT_ID();";
                    var id = await connection.QuerySingleAsync<int>(getIdSql);
                    return $"{id}";
                }
                catch (Exception ex) when (currentTry < maxRetries - 1)
                {
                    currentTry++;
                    Console.WriteLine($"SeedExercise attempt {currentTry} failed: {ex.Message}. Retrying...");
                    await Task.Delay(500 * currentTry);
                }
            }

            throw new Exception($"Failed to seed exercise after {maxRetries} attempts");
        }
    }
}
