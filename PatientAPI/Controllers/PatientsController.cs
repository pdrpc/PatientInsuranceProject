using Application.DTOs.Patient;
using Application.Services;
using Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PatientAPI.Controllers
{
    public class PatientsController : BaseApiController
    {
        private readonly IPatientService _service;

        public PatientsController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await _service.GetPatientsAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _service.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto createDto)
        {

            try
            {
                var newPatient = await _service.CreatePatientAsync(createDto);
                return CreatedAtAction(nameof(GetPatientById), new { id = newPatient.Id }, newPatient);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] UpdatePatientDto updateDto)
        {
            try
            {
                var success = await _service.UpdatePatientAsync(id, updateDto);
                if (!success)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var success = await _service.DeletePatientAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
