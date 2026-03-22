using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;
using System.Text.Json;

namespace Smart_Desk_AI.Agents
{
    public class BookingAgent
    {
        private readonly OpenAIService _openAI;
        private readonly string _persona;
        private readonly AgentConfig _config;

        public BookingAgent(
            OpenAIService openAI,
            string persona,
            AgentConfig config)
        {
            _openAI = openAI;
            _persona = persona;
            _config = config;
        }

        public async Task<AgentDecision> ProcessBookingAsync(
            string customerMessage,
            string availableSlots,
            string? customerHistory = null)
        {
            var historyPart = customerHistory != null
                ? "CUSTOMER HISTORY:\n" + customerHistory + "\n\n"
                : "";

            var prompt = "AVAILABLE SLOTS:\n" +
                availableSlots + "\n\n" +
                historyPart +
                "CUSTOMER REQUEST:\n" +
                customerMessage + "\n\n" +
                "Check availability and process this booking request. " +
                "Return JSON only.";

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
            Reason = "Could not process booking",
            ConfidenceScore = 0
        };
    }
}