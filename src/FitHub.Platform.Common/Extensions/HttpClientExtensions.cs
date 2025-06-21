using System.Text;
using System.Text.Json;

namespace FitHub.Platform.Common.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static async Task<HttpResponseMessage> PostAsJsonAsync<TEntity>(this HttpClient client, string requestUri, TEntity value)
        {
            var json = JsonSerializer.Serialize(value, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "appliction/json");
            return await client.PostAsync(requestUri, content);
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            var json = JsonSerializer.Serialize(value, JsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await client.PutAsync(requestUri, content);
        }

        public static async Task<TEntity?> ReadAsJsonAsync<TEntity>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TEntity>(json, JsonOptions);
        }
    }
}
