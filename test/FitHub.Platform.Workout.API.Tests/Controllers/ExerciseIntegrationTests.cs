using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace FitHub.Platform.Workout.API.Tests.Controllers
{
    public class ExerciseIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ExerciseIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Exercises_ReturnsOk()
        {
            //Arrange

            //Act
            var response = await _client.GetAsync("/api/exercise");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
