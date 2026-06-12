using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [HttpPost("DoctorRegister")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromForm] CreateDoctor createDoctor, CancellationToken cancellationToken)
        {
            var result = await doctorService.DoctorRegister(createDoctor, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetDoctor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetDoctor([FromBody] int DoctorId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetDoctor(DoctorId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("UpdateDoctor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDoctor([FromBody]UpdateDoctor updateDoctor, CancellationToken cancellationToken)
        {
            var result = await doctorService.UpdateDoctor(updateDoctor, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteDoctor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDoctor([FromBody]DeleteDoctor deleteDoctor, CancellationToken cancellationToken)
        {
            var result = await doctorService.DeleteDoctor(deleteDoctor, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
