using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskMicroservice.Application.DTOs;
using RiskMicroservice.Application.Interfaces;
using System.Threading.Tasks;

namespace RiskMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RiskController : ControllerBase
    {
        private readonly IRiskService _riskService;

        public RiskController(IRiskService riskService)
        {
            _riskService = riskService;
        }

        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetRisk(int patientId)
        {
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrWhiteSpace(jwtToken))
                return Unauthorized();

            var result = await _riskService.AssessmentRiskAsync(patientId, jwtToken);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }

}
