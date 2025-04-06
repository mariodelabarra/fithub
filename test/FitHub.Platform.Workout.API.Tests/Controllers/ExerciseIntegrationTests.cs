using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitHub.Platform.Workout.API;
using FluentAssertions;
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
            //Act
            var response = await _client.GetAsync("/api/exercise");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
