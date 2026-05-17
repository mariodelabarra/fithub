using AutoMapper;
using Fithub.Platform.Domain.Workout.In;
using Fithub.Platform.Repositories;
using Fithub.Platform.Repositories.Workout;
using Fithub.Platform.Services.Mapping;
using Fithub.Platform.Services.Workout;
using FitHub.Platform.Common.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FitHub.Platform.API;

public static class DependencyInjection
{
    public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
    {
        RegisterAuthentication(services, configuration);
        RegisterConfiguration(services, configuration);
        RegisterServices(services);
        RegisterRepositories(services, configuration);
    }

    public static void RegisterAuthentication(IServiceCollection services, ConfigurationManager configuration)
    {
        var tenantId = configuration["Agglestone:Auth:TenantId"];
        var authority = $"https://auth.agglestone.com/tenant/{tenantId}/v2/Auth";

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Cookie.Name = "AggleStone.Auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromHours(1);
            options.SlidingExpiration = true;
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.Authority = authority;
            options.ClientId = tenantId;
            // NO ClientSecret for public client with PKCE
            options.ResponseType = "code";
            options.UsePkce = true;
            options.ResponseMode = "query";

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            options.SaveTokens = true; // Save tokens in authentication properties
            options.GetClaimsFromUserInfoEndpoint = true;

            options.CallbackPath = "/auth/callback";
            options.SignedOutCallbackPath = "/auth/signout-callback";

            //Map claims
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });
    }

    public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<FithubDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

        // Problem Details
        services.AddProblemDetails();

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddValidatorsFromAssemblyContaining<CreateExerciseInValidator>();
        services.AddFluentValidationAutoValidation();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        //Mapper
        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new WorkoutProfile());
        }, loggerFactory);
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        //Services
        services.AddScoped<IExerciseService, ExerciseService>();

        //Validators
        services.AddTransient<IValidatorService, ValidatorService>();
    }

    public static void RegisterRepositories(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
    }
}
