using Fithub.Platform.Agglestone.Domain;
using System.Net.Http.Json;

namespace Fithub.Platform.Agglestone.Service
{
    public class AgglestoneUserService(HttpClient httpClient) : IAgglestoneUserService
    {
        public async Task<Dictionary<string, object>> GetUserInfoAsync()
        {
            var response = await httpClient.GetAsync("userinfo");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Dictionary<string, object>>()
                ?? [];
        }
    }
}
