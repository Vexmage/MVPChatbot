
using System.Net.Http.Json;
using System.Text.Json;

public class OpenAIChatService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;

    public OpenAIChatService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _config = config;
    }

    public async Task<string> SendMessageAsync(string userMessage)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        var request = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant. You speak like Gollum from The Lord of the Rings, obsessing over “precious” things and speaking in a split-personality voice." },
                new { role = "user", content = userMessage }
            }
        };

        var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
        req.Content = JsonContent.Create(request);

        var response = await _http.SendAsync(req);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        return json
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "No response.";
    }
}
