using AutoMapper;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;
using FitHub.Platform.Workout.Service;
using FitHub.Platform.Workout.Service.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FitHub.Platform.Workout.API
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            RegisterAuthentication(services, configuration);
            RegisterConfiguration(services, configuration);
            RegisterServices(services);
            RegisterRepositories(services);
        }

        public static void RegisterAuthentication(IServiceCollection services, ConfigurationManager configuration)
        {
            var tenantId = configuration["Agglestone:Auth:TenantId"];
            var authority = $"https://auth.agglestone.com/tenant/{tenantId}/v2/auth";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = tenantId;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
        }

        public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IExerciseRepository>(provider => new ExerciseRepository(configuration));

            services.AddProblemDetails();

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

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WorkoutProfile());
            }, loggerFactory);
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IExerciseService, ExerciseService>();

            services.AddTransient<IValidatorService, ValidatorService>();
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            DapperTypeHandlerInitializer.RegisterHandlers();
        }
    }
}
