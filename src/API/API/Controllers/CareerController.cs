using API.Controllers.Interfaces;
using API.Models.DTOs.Career;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CareerController : ControllerBase, ICareerController
    {
        private readonly CareerService _service;
        public CareerController(CareerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCareerAsync()
        {
            var careers = await _service.GetAllCareerAsync();

            if (careers is null || careers.Count() == 0)
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
            actionName: "GetCareerById",                  //endpoint usado para buscar esse item
            routeValues: new { id = newCareerId },        //Preenche a rota automaticamente
            value: createdCareer);                        //Retorna o objeto criado no corpo
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
