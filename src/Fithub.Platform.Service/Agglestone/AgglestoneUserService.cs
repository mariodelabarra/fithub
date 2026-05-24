using System.Net.Http.Json;
using Fithub.Platform.Domain.Agglestone;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Fithub.Platform.Services.Agglestone;

public class AgglestoneUserService(IHttpClientFactory httpClientFactory, IMemoryCache cache, IOptions<AgglestoneSettings> settings) : IAgglestoneUserService
{
    private readonly string _tenantId = settings.Value.TenantId;

    public async Task<AgglestoneUserDto?> GetUserAsync(string userId, CancellationToken ct = default)
    {
        var cacheKey = $"agglestone:user:{userId}";
        if (cache.TryGetValue(cacheKey, out AgglestoneUserDto? cached))
        {
            return cached;
        }

        var client = httpClientFactory.CreateClient("agglestone");
        var response = await client.GetAsync($"tenant/{_tenantId}/v2/Users/{userId}", ct);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var apiUser = await response.Content.ReadFromJsonAsync<AgglestoneUserApiResponse>(ct);
        if (apiUser is null)
        {
            return null;
        }

        var result = new AgglestoneUserDto(apiUser.Id ?? userId, apiUser.Email ?? string.Empty, apiUser.DisplayName);

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
        return result;
    }

    private sealed record AgglestoneUserApiResponse(string? Id, string? Email, string? DisplayName);
}
