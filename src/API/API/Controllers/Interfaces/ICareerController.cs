using API.Models.DTOs.Career;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Interfaces
{
    public interface ICareerController
    {
        Task<IActionResult> GetAllCareerAsync();

        Task<IActionResult> GetCareerById(Guid id);

        Task<IActionResult> CreateCareerAsync([FromBody] CareerRequestDTO career);

        Task<IActionResult> UpdateCareerAsync(Guid id, [FromBody] CareerRequestDTO career);
    }
}
