using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientMicroservice.Application.DTOs;
using PatientMicroservice.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientMicroservice.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
        
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient ==  null)
                return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> Create([FromBody] PatientDto patientDto)
        {
            await _patientService.AddPatientAsync(patientDto);
            return CreatedAtAction(nameof(GetById), new { id = patientDto.Id }, patientDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
        {
            var existing = await _patientService.GetPatientByIdAsync(id);
            if (existing == null)
                return NotFound();
            await _patientService.UpdatePatientAsync(id, patientDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exsiting = await _patientService.GetPatientByIdAsync(id);
            if (exsiting == null)
                return NotFound();
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
