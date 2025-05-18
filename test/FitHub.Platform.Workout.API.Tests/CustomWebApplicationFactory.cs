using FitHub.Platform.Workout.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MySql;

namespace FitHub.Platform.Workout.API.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MySqlContainer _dbContainer = new MySqlBuilder()
            .WithImage("mysql:latest")
            .WithDatabase("test")
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();

        public HttpClient HttpClient { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            HttpClient = CreateClient();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ConnectionString:DefaultConnection", _dbContainer.GetConnectionString());
        }
    }
}
