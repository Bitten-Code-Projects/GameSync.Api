using System.Text.Json;

namespace GameSync.Api.IntegrationTests.Utilities;

public static class JsonUtilities<T>
    where T : class
{
    private static readonly JsonSerializerOptions _defaultOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
    };

    public async static Task<T?> DeserializeResponse(HttpResponseMessage response, JsonSerializerOptions options = null!)
    {
        var rawContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T?>(rawContent, options is null ? _defaultOptions : options);
    }
}
