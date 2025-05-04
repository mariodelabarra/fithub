using AutoMapper;
using FitHub.Platform.Common.Repository;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;
using FitHub.Platform.Workout.Service;
using FitHub.Platform.Workout.Service.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace FitHub.Platform.Workout.API
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection services, ConfigurationManager configuration)
        {
            RegisterConfiguration(services, configuration);
            RegisterServices(services);
            RegisterRepositories(services);
        }

        public static void RegisterConfiguration(IServiceCollection services, ConfigurationManager configuration)
        {

            // Add services to the container.
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Exercise>("exercises").EntityType.HasKey(c => c.Id);
            services.AddControllers()
                .AddOData(
                opt =>
                {
                    opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(1000).AddRouteComponents(
                        "odata",
                        modelBuilder.GetEdmModel());
                });

            //MySql Connection
            services.AddScoped<IExerciseRepository>(provider => new ExerciseRepository(configuration));

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
            //Mapper
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WorkoutProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //Services
            services.AddScoped<IExerciseService, ExerciseService>();

            //Validators
            services.AddTransient<IValidatorService, ValidatorService>();
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            DapperTypeHandlerInitializer.RegisterHandlers();
        }
    }
}
