using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;
using System.Text.Json;

namespace Smart_Desk_AI.Agents
{
    public class DynamicAgent
    {
        private readonly OpenAIService _openAI;
        private readonly string _persona;
        private readonly AgentConfig _config;

        public DynamicAgent(
            OpenAIService openAI,
            string persona,
            AgentConfig config)
        {
            _openAI = openAI;
            _persona = persona;
            _config = config;
        }

        public async Task<AgentDecision> ProcessAsync(
            string userMessage,
            string? context = null)
        {
            var contextPart = context != null
                ? "CONTEXT:\n" + context + "\n\n"
                : "";

            var prompt = contextPart +
                "CUSTOMER MESSAGE:\n" +
                userMessage + "\n\n" +
                "Process this request according to your rules " +
                "and return your response as JSON only.";

            var response = await _openAI.ChatAsync(_persona, prompt);
            return ParseDecision(response);
        }

        private AgentDecision ParseDecision(string response)
        {
            try
            {
                var start = response.IndexOf('{');
                var end = response.LastIndexOf('}') + 1;
                var json = response.Substring(start, end - start);
                return JsonSerializer.Deserialize<AgentDecision>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? Fallback();
            }
            catch
            {
                return Fallback();
            }
        }

        private AgentDecision Fallback() => new AgentDecision
        {
            AgentName = _config.AgentName,
            Decision = "ESCALATE",
            Reason = "Could not process request",
            ConfidenceScore = 0
        };
    }
}