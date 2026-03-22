using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Desk_AI.Agents;
using Smart_Desk_AI.Data;
using Smart_Desk_AI.Models;

namespace Smart_Desk_AI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentConfigController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AgentFactory _factory;

        public AgentConfigController(
            AppDbContext db,
            AgentFactory factory)
        {
            _db = db;
            _factory = factory;
        }

        // Get all agent configs for a business
        [HttpGet("{businessId}")]
        public async Task<IActionResult> GetConfigs(int businessId)
        {
            var configs = await _db.AgentConfigs
                .Where(c => c.BusinessId == businessId)
                .ToListAsync();
            return Ok(configs);
        }

        // Create or update agent config
        [HttpPost]
        public async Task<IActionResult> SaveConfig(
            [FromBody] AgentConfig config)
        {
            if (config.Id == 0)
            {
                config.CreatedAt = DateTime.UtcNow;
                _db.AgentConfigs.Add(config);
            }
            else
            {
                _db.AgentConfigs.Update(config);
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Agent " + config.AgentName + " saved!",
                config = config
            });
        }

        // Test your custom agent
        [HttpPost("{id}/test")]
        public async Task<IActionResult> TestAgent(
            int id, [FromBody] TestAgentRequest request)
        {
            var config = await _db.AgentConfigs
                .Include(c => c.Business)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (config == null)
                return NotFound("Agent config not found");

            if (config.Business == null)
                return NotFound("Business not found");

            // Create dynamic agent from config
            var agent = _factory.CreateDynamicAgent(
                config, config.Business);

            // Process the test message
            var decision = await agent.ProcessAsync(
                request.Message, request.Context);

            return Ok(new
            {
                agentName = config.AgentName,
                personality = config.Personality,
                response = decision
            });
        }

        // Delete agent config
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfig(int id)
        {
            var config = await _db.AgentConfigs.FindAsync(id);
            if (config == null)
                return NotFound();

            _db.AgentConfigs.Remove(config);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Agent deleted" });
        }
    }
}