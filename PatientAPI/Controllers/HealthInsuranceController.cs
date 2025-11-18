using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PatientAPI.Controllers
{
    public class HealthInsuranceController : BaseApiController
    {
        private readonly IHealthInsuranceService _service;

        public HealthInsuranceController(IHealthInsuranceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var insurances = await _service.GetAllAsync();
            return Ok(insurances);
        }
    }
}
