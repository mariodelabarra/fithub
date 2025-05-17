using Bogus;
using FitHub.Platform.Workout.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    public class ExerciseControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient _client;

        protected Faker _faker = new();

        public ExerciseControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        public class GetAllIntegrationTests(WebApplicationFactory<Program> factory) : ExerciseControllerTests(factory)
        {
            [Fact]
            public async Task GetAll_ReturnsOk()
            {
                //Arrange

                //Act
                var response = await _client.GetAsync("/api/exercise");

                //Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);                
            }
        }

        public class GetByIdIntegrationTests(WebApplicationFactory<Program> factory) : ExerciseControllerTests(factory)
        {
            [Fact]
            public async Task GetById_ReturnsOk()
            {
                //Arrange
                var id = _faker.Random.Number();

                //Act
                var response = await _client.GetAsync($"/api/exercise/{id}");
                var content = await response.Content.ReadAsStringAsync();
                var exercise = JsonSerializer.Deserialize<Exercise>(content);

                //Assert
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                exercise.Should().NotBeNull();
                //exercise.Id.Should().Be(id);
            }
        }

    }
}
