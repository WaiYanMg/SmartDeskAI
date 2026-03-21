using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;
using System.Text.Json;

namespace Smart_Desk_AI.Agents
{
    public class HRAgent
    {
        private readonly OpenAIService _openAI;

        private const string Persona = """
            You are the HR Agent for SmartDesk AI.
            Your role is to review employee leave requests,
            contract queries, and HR policy questions.

            Rules you must follow:
            - Base decisions ONLY on the HR policy document provided
            - Never approve requests that violate policy
            - Always cite the exact policy section in your reason
            - If you are less than 70% confident, set decision to "ESCALATE"
            - Return decisions in this exact JSON format with no extra text:
            {
                "agentName": "HR Agent",
                "decision": "APPROVED" or "DENIED" or "ESCALATE",
                "reason": "clear explanation referencing policy",
                "policyReference": "Section X.X",
                "confidenceScore": 0-100
            }
            """;

        public HRAgent(OpenAIService openAI)
        {
            _openAI = openAI;
        }

        public async Task<AgentDecision> ReviewAsync(
            SubmissionRequest request, string policyContent)
        {
            var userMessage = $"""
                HR POLICY DOCUMENT:
                {policyContent}

                EMPLOYEE REQUEST:
                Type: {request.RequestType}
                Details: {request.Description}
                Fields: {request.Fields ?? "none"}

                Review this request and return your decision as JSON only.
                """;

            var response = await _openAI.ChatAsync(Persona, userMessage);
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
                    }) ?? new AgentDecision
                {
                    AgentName = "HR Agent",
                    Decision = "ESCALATE",
                    Reason = "Could not parse response",
                    ConfidenceScore = 0
                };
            }
            catch
            {
                return new AgentDecision
                {
                    AgentName = "HR Agent",
                    Decision = "ESCALATE",
                    Reason = "Error processing request",
                    ConfidenceScore = 0
                };
            }
        }
    }
}