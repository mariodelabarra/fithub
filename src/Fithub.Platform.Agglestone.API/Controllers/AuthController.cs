using Fithub.Platform.Agglestone.Domain.Out;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fithub.Platform.Agglestone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController() : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            var redirectUri = "http://localhost:4200";
            if (!string.IsNullOrEmpty(returnUrl))
                redirectUri = $"http://localhost:4200{returnUrl}";

            var properties = new AuthenticationProperties { RedirectUri = redirectUri };
            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var properties = new AuthenticationProperties { RedirectUri = "http://localhost:4200" };
            return SignOut(properties,
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet("user")]
        public ActionResult<GetUserOut> GetUser()
        {
            if (User.Identity?.IsAuthenticated != true)
                return Ok(new GetUserOut(false, null));

            var claims = User.Claims.Select(c => new ClaimsOut(c.Type, c.Value));

            var userDetails = new UserDetailsOut(
                User.Identity.Name!,
                User.FindFirst("email")?.Value!,
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value!,
                claims);

            return Ok(new GetUserOut(true, userDetails));
        }

        [HttpGet("token")]
        [Authorize]
        public async Task<IActionResult> GetToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized(new { error = "No access token in session" });

            return Ok(new { access_token = accessToken });
        }

        [HttpGet("callback")]
        public IActionResult Callback() => Redirect("http://localhost:4200");

        [HttpGet("signout-callback")]
        public IActionResult SignoutCallback() => Redirect("http://localhost:4200");
    }
}
