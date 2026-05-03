using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FitHub.Platform.Workout.API.Controllers
{
    [Route("api/proxy")]
    [ApiController]
    [Authorize]
    public class AgglestoneProxyController(IHttpClientFactory _httpClientFactory,
        IConfiguration _configuration) : ControllerBase
    {
        [HttpGet("{**path}")]
        [HttpPost("{**path}")]
        [HttpPut("{**path}")]
        [HttpDelete("{**path}")]
        [HttpPatch("{**path}")]
        public async Task<IActionResult> ProxyRequest(string path)
        {
            // Get the access token from the authenticated user
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { error = "No access token available" });
            }

            Console.Write(accessToken);

            // Build the target URL - you'll need to configure your actual API base URL
            // This could come from appsettings if your API is on a different domain than the auth server
            var targetUrl = $"https://auth.agglestone.com/tenant/{_configuration["Agglestone:Auth:TenantId"]}/v2/{path}";

            // Add query string if present
            if (Request.QueryString.HasValue)
            {
                targetUrl += Request.QueryString.Value;
            }

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(new HttpMethod(Request.Method), targetUrl);

            // Add the access token from authentication
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Copy relevant headers (excluding some that shouldn't be forwarded)
            foreach (var header in Request.Headers)
            {
                if (!header.Key.StartsWith("Content-") &&
                    header.Key != "Host" &&
                    header.Key != "Cookie" &&
                    header.Key != "Authorization")
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            // Copy body for non-GET requests
            if (Request.Method != "GET" && Request.ContentLength > 0)
            {
                var content = new StreamContent(Request.Body);
                if (Request.ContentType != null)
                {
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse(Request.ContentType);
                }
                request.Content = content;
            }

            try
            {
                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                return new ContentResult
                {
                    StatusCode = (int)response.StatusCode,
                    Content = responseContent,
                    ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json"
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Proxy request failed", details = ex.Message });
            }
        }
    }
}
