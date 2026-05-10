using Fithub.Platform.Agglestone.Domain;
using Fithub.Platform.Agglestone.Service.HttpClients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Fithub.Platform.Agglestone.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgglestoneModule(this IServiceCollection services, IConfiguration configuration)
        {
            var tenantId = configuration["Agglestone:Auth:TenantId"];
            var authority = $"https://auth.agglestone.com/tenant/{tenantId}/v2/auth";
            var apiBase = $"https://auth.agglestone.com/tenant/{tenantId}/v2/";

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
                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "form_post";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.CallbackPath = "/auth/callback";
                options.SignedOutCallbackPath = "/auth/signout-callback";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

            services.AddHttpContextAccessor();
            services.AddTransient<AgglestoneHttpClient>();

            services.AddHttpClient<IAgglestoneUserService, AgglestoneUserService>(client =>
            {
                client.BaseAddress = new Uri(apiBase);
            })
            .AddHttpMessageHandler<AgglestoneHttpClient>();

            return services;
        }
    }
}
