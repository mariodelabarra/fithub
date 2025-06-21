using DotNet.Testcontainers.Builders;
using FitHub.Platform.Workout.Repository.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;

namespace FitHub.Platform.Workout.API.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private MySqlContainer _dbContainer;

        public HttpClient HttpClient { get; private set; } = null!;

        public string GetConnectionString() => _dbContainer.GetConnectionString();

        public async Task InitializeAsync()
        {
            _dbContainer = new MySqlBuilder()
                .WithImage("mysql:8.0")
                .WithDatabase("test")
                .WithUsername("admin")
                .WithPassword("admin")
                .WithCleanUp(true)
                .WithReuse(true)
                .WithPortBinding(3306, true) // Let Docker assign a random port
                .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
                .Build();

            await _dbContainer.StartAsync();
            HttpClient = CreateClient();

            MigrateDatabase();
        }

        private void MigrateDatabase()
        {
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb.AddMySql()
                    .WithGlobalConnectionString(_dbContainer.GetConnectionString())
                    .ScanIn(typeof(CreateExerciseTable).Assembly).For.Migrations())
                .BuildServiceProvider();

            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IConfiguration));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                var configData = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString() ?? "Server=localhost;Database=testdb;Uid=testuser;Pwd=testpass;"
                };

                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(configData)
                    .Build();

                services.AddSingleton<IConfiguration>(config);
            });
            builder.UseEnvironment("Testing");
        }

        public new async Task DisposeAsync()
        {
            if (_dbContainer is not null)
            {
                try
                {
                    await _dbContainer.DisposeAsync();
                }
                catch (ObjectDisposedException)
                {
                    // Container already disposed, ignore
                }
            }
        }
    }
}
