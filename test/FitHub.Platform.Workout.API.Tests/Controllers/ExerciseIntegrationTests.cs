using Bogus;
using FitHub.Platform.Workout.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    public class ExerciseControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        protected Faker _faker = new();

        public ExerciseControllerTests(CustomWebApplicationFactory factory)
        {
        }

        [Collection("GetByIdExerciseTests")]
        public class GetByIdIntegrationTests : ExerciseControllerTests, IAsyncLifetime
        {
            private readonly CustomWebApplicationFactory _factory;

            public GetByIdIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
            {
                _factory = factory;
            }

            public Task InitializeAsync() => Task.CompletedTask;
            public Task DisposeAsync() => Task.CompletedTask;

            [Fact]
            public async Task GetById_ReturnsOk()
            {
                //Arrange
                var id = _faker.Random.Number();

                //Act
                var response = await _factory.HttpClient.GetAsync($"/api/exercise/{id}");
                var content = await response.Content.ReadAsStringAsync();
                var exercise = JsonSerializer.Deserialize<Exercise>(content);

                //Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                exercise.Should().NotBeNull();
            }
        }

    }
}
