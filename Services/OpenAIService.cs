using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Smart_Desk_AI.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;

        public OpenAIService(IConfiguration config)
        {
            var apiKey = config["Groq:Key"]!;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> ChatAsync(string systemPrompt, string userMessage)
        {
            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userMessage }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions", content);

            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Groq error: {responseJson}");

            using var doc = JsonDocument.Parse(responseJson);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;
        }
    }
}