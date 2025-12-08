using DevLearning.Models.DTOs.Career;
using DevLearning.CareerAPI.Service;
using DevLearning.CareerAPI.Service.Interfaces;
using DevLearning.Models.DTOs.Career;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DevLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CareerController : ControllerBase
    {
        private readonly ICareerService _service;
        public CareerController(ICareerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCareerAsync()
        {
            var careers = await _service.GetAllCareerAsync();

            if (careers is null || !careers.Any())
            {
                return NotFound();
            }

            return Ok(careers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCareerById(Guid id)
        {
            var career = await _service.GetCareerByIdAsync(id);

            if (career == null)
            {
                return NotFound();
            }
            return Ok(career);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCareerAsync([FromBody] CareerRequestDTO career)
        {
            var newCareerId = await _service.CreateCareerAsync(career);
            var createdCareer = await _service.GetCareerByIdAsync(newCareerId);

            return CreatedAtAction(
            actionName: nameof(GetCareerById),                  
            routeValues: new { id = newCareerId },       
            value: createdCareer);                        
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCareerAsync(Guid id, [FromBody] CareerRequestDTO career)
        {
            var updated = await _service.UpdateCareerAsync(id, career);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
