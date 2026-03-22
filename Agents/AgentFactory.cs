using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;

namespace Smart_Desk_AI.Agents
{
    public class AgentFactory
    {
        private readonly OpenAIService _openAI;

        public AgentFactory(OpenAIService openAI)
        {
            _openAI = openAI;
        }

        public BookingAgent CreateBookingAgent(
            AgentConfig config, Business business)
        {
            var persona = BuildPersona(config, business);
            return new BookingAgent(_openAI, persona, config);
        }

        public DynamicAgent CreateDynamicAgent(
            AgentConfig config, Business business)
        {
            var persona = BuildPersona(config, business);
            return new DynamicAgent(_openAI, persona, config);
        }

        private string BuildPersona(
            AgentConfig config, Business business)
        {
            return "You are " + config.AgentName + ", an AI assistant " +
                   "for " + business.Name + ".\n\n" +
                   "Your personality is: " + config.Personality + "\n" +
                   "Your tone is: " + config.Tone + "\n" +
                   "Business type: " + business.Type + "\n" +
                   "Your primary task: " + config.Task + "\n\n" +
                   "Custom rules you MUST always follow:\n" +
                   config.CustomRules + "\n\n" +
                   "Business hours: " + business.OpeningTime +
                   " - " + business.ClosingTime + "\n\n" +
                   "Welcome message: " + config.WelcomeMessage + "\n\n" +
                   "Language: " + config.Language + "\n\n" +
                   "IMPORTANT RULES:\n" +
                   "- Always stay in character as " + config.AgentName + "\n" +
                   "- Never break the rules set by the business owner\n" +
                   "- Always be helpful within your defined role\n" +
                   "- If unsure, escalate to human staff\n" +
                   "- Return decisions in this exact JSON format:\n" +
                   "{\n" +
                   "  \"agentName\": \"" + config.AgentName + "\",\n" +
                   "  \"decision\": \"CONFIRMED or DENIED or ESCALATE or MORE_INFO\",\n" +
                   "  \"reason\": \"clear explanation\",\n" +
                   "  \"message\": \"friendly message to customer\",\n" +
                   "  \"confidenceScore\": 0-100\n" +
                   "}";
        }
    }
}