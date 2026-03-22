using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Data;
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
        private readonly AgentFactory _agentFactory;
        private readonly AppDbContext _db;

        public OrchestratorAgent(
            HRAgent hrAgent,
            FinanceAgent financeAgent,
            ITAgent itAgent,
            DocumentService documentService,
            AgentFactory agentFactory,
            AppDbContext db)
        {
            _hrAgent = hrAgent;
            _financeAgent = financeAgent;
            _itAgent = itAgent;
            _documentService = documentService;
            _agentFactory = agentFactory;
            _db = db;
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
                    decisions.Add(await ProcessBookingAsync(request));
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

        private async Task<AgentDecision> ProcessBookingAsync(
            SubmissionRequest request)
        {
            // Find active booking agent config
            var config = await _db.AgentConfigs
                .Include(c => c.Business)
                .Where(c => c.IsActive &&
                       c.Task != null &&
                       c.Task.ToLower().Contains("booking"))
                .FirstOrDefaultAsync();

            if (config == null || config.Business == null)
            {
                return new AgentDecision
                {
                    AgentName = "Booking Agent",
                    Decision = "ESCALATE",
                    Reason = "No booking agent configured. " +
                             "Please set up a Booking Agent " +
                             "in Agent Config page.",
                    PolicyReference = "N/A",
                    ConfidenceScore = 0
                };
            }

            // Build booking agent from config
            var bookingAgent = _agentFactory
                .CreateBookingAgent(config, config.Business);

            // Process booking
            var decision = await bookingAgent.ProcessBookingAsync(
                request.Description,
                request.Fields ?? "No specific slots provided",
                null);

            return decision;
        }
    }
}