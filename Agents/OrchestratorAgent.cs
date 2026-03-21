using Smart_Desk_AI.Models;
using Smart_Desk_AI.Services;

namespace Smart_Desk_AI.Agents
{
    public class OrchestratorAgent
    {
        private readonly HRAgent _hrAgent;
        private readonly FinanceAgent _financeAgent;
        private readonly ITAgent _itAgent;
        private readonly DocumentService _documentService;

        public OrchestratorAgent(
            HRAgent hrAgent,
            FinanceAgent financeAgent,
            ITAgent itAgent,
            DocumentService documentService)
        {
            _hrAgent = hrAgent;
            _financeAgent = financeAgent;
            _itAgent = itAgent;
            _documentService = documentService;
        }

        public async Task<List<AgentDecision>> ProcessAsync(
            SubmissionRequest request)
        {
            var decisions = new List<AgentDecision>();

            switch (request.RequestType.ToLower())
            {
                case "leave":
                case "hr":
                    var hrPolicy = await _documentService
                        .LoadPolicyAsync("hr-policy.txt");
                    decisions.Add(await _hrAgent
                        .ReviewAsync(request, hrPolicy));
                    break;

                case "expense":
                case "claim":
                    var financePolicy = await _documentService
                        .LoadPolicyAsync("finance-policy.txt");
                    decisions.Add(await _financeAgent
                        .ReviewAsync(request, financePolicy));
                    break;

                case "it":
                case "access":
                case "hardware":
                    var itPolicy = await _documentService
                        .LoadPolicyAsync("it-policy.txt");
                    decisions.Add(await _itAgent
                        .ReviewAsync(request, itPolicy));
                    break;

                case "booking":
                    var hrPol = await _documentService
                        .LoadPolicyAsync("hr-policy.txt");
                    var finPol = await _documentService
                        .LoadPolicyAsync("finance-policy.txt");
                    decisions.Add(await _hrAgent
                        .ReviewAsync(request, hrPol));
                    decisions.Add(await _financeAgent
                        .ReviewAsync(request, finPol));
                    break;

                default:
                    decisions.Add(new AgentDecision
                    {
                        AgentName = "Orchestrator",
                        Decision = "ESCALATE",
                        Reason = "Unknown request type. " +
                                 "Please specify: leave, expense, " +
                                 "claim, booking, it, access, hardware",
                        ConfidenceScore = 0
                    });
                    break;
            }

            return decisions;
        }
    }
}