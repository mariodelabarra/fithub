using DotNet.Testcontainers.Builders;
using Fithub.Platform.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;

namespace Fithub.Platform.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MySqlContainer? _dbContainer;

    public HttpClient HttpClient { get; private set; } = null!;

    public string GetConnectionString() => _dbContainer!.GetConnectionString();

    public async Task InitializeAsync()
    {
        _dbContainer = new MySqlBuilder()
            .WithImage("mysql:8.0")
            .WithDatabase("test")
            .WithUsername("admin")
            .WithPassword("admin")
            .WithCleanUp(true)
            .WithReuse(true)
            .WithPortBinding(3306, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(3306))
            .Build();

        await _dbContainer.StartAsync();
        HttpClient = CreateClient();

        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FithubDbContext>();
        await context.Database.EnsureCreatedAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FithubDbContext>));
            if (descriptor is not null)
                services.Remove(descriptor);

            var connectionString = _dbContainer!.GetConnectionString();
            services.AddDbContext<FithubDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));
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
