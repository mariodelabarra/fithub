using AutoMapper;
using FitHub.Platform.Common.Repository;
using FitHub.Platform.Workout.Repository;
using FitHub.Platform.Workout.Service;
using FitHub.Platform.Workout.Service.Mapping;

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
            // MongoDB config
            MongoDbContext.Initialize("fithubworkout", configuration.GetConnectionString("MongoDbConnection")!).Wait();

            // Problem Details
            services.AddProblemDetails();
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
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IExerciseRepository, ExerciseRepository>();
        }
    }
}
