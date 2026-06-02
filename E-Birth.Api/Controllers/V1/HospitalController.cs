using E_Birth.Application.Authservice;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HospitalController:ControllerBase
    {
        private readonly IHospitalService hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            this.hospitalService = hospitalService;
        }
        [HttpPost("CreateHospital")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHospital(CreateHospital createHospital, CancellationToken cancellationToken)
        {
            var result = await hospitalService.HospitalRegister(createHospital, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetHospital")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHospital(GetHospitalRequest getHospitalRequest, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetHospital(getHospitalRequest, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
       
        [HttpDelete("DeleteHospital")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHospital(DeleteHospitalRequest deleteHospitalRequest, CancellationToken cancellationToken)
        {
            var result = await hospitalService.DeleteHospital(deleteHospitalRequest, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


    }
}
