using AutoMapper;
using FitHub.Platform.Common.Service;
using FitHub.Platform.Workout.Domain;
using FitHub.Platform.Workout.Repository;
using FitHub.Platform.Workout.Service;
using FitHub.Platform.Workout.Service.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;

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
                options.Cookie.SameSite = SameSiteMode.Strict;
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
                options.ResponseMode = "form_post";

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

        public static void RegisterRepositories(IServiceCollection services)
        {
            //services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IExerciseRepository, ExerciseRepository>();
            DapperTypeHandlerInitializer.RegisterHandlers();
        }
    }
}
