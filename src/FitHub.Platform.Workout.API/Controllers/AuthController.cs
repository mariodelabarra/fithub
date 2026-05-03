using FitHub.Platform.Workout.Domain.Out;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitHub.Platform.Workout.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController() : ControllerBase
    {

        [HttpGet("login")]
        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            // After authentication, redirect back to Angular app
            var redirectUri = "http://localhost:4200";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                redirectUri = $"http://localhost:4200{returnUrl}";
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };

            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "http://localhost:4200"
            };

            return SignOut(properties,
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("user")]
        public ActionResult<GetUserOut> GetUser()
        {
            if(!User.Identity?.IsAuthenticated ?? false)
            {
                return Ok(new { isAuthenticated = false });
            }

            var claims = User.Claims.Select(c => new ClaimsOut(c.Type, c.Value));

            var userDetailsOut = new UserDetailsOut
            (
                User.Identity?.Name!,
                User.FindFirst("email")?.Value!,
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                claims
            );

            return Ok(new GetUserOut(true, userDetailsOut));
        }

        [HttpGet("callback")]
        public IActionResult Callback()
        {
            // OpenID Connect middleware handles this automatically
            // Just redirect to Angular app
            return Redirect("http://localhost:4200");
        }

        [HttpGet("signout-callback")]
        public IActionResult SignoutCallback()
        {
            return Redirect("http://localhost:4200");
        }
    }
}
