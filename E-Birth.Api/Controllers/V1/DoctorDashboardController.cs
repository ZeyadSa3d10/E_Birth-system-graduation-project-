using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(Roles = "Doctor")]
    public class DoctorDashboardController : ControllerBase
    {
        private readonly IDoctorService doctorService;

        public DoctorDashboardController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetDoctorForDashboard")]
        public async Task<IActionResult> GetDoctorForDashboard([FromBody] string DoctorAspNetUserId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetDoctorForDashboard(DoctorAspNetUserId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetUserDetailsAsync")]
        public async Task<IActionResult> GetUserDetailsAsync([FromBody] string NationalId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetUserDetailsAsync(NationalId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetChildVaccinationsAsync")]
        public async Task<IActionResult> GetChildVaccinationsAsync([FromBody] int ChildId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetChildVaccinationsAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetChildMedicalHistoryAsync")]
        public async Task<IActionResult> GetChildMedicalHistoryAsync([FromBody] int ChildId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetChildMedicalHistoryAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetSpecificChildMedicalHistoryAsync")]
        public async Task<IActionResult> GetSpecificChildMedicalHistoryAsync([FromBody] int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetSpecificChildMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("AddChildMedicalRecord")]
        public async Task<IActionResult> AddChildMedicalRecord(AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken)
        {
            var result = await doctorService.AddChildMedicalRecord(addUserMedicalRecord, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }



        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetParentMedicalHistoryAsync([FromBody] int ParentId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetParentMedicalHistoryAsync(ParentId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetSpecificParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetSpecificParentMedicalHistoryAsync([FromBody] int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetSpecificParentMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }



        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("AddParentMedicalRecord")]
        public async Task<IActionResult> AddParentMedicalRecord( AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken)
        {
            var result = await doctorService.AddParentMedicalRecord(addUserMedicalRecord, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }



        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromBody] int DoctorId, CancellationToken cancellationToken)
        {
            var result = await doctorService.GetDoctor(DoctorId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctor updateDoctor, CancellationToken cancellationToken)
        {
            var result = await doctorService.UpdateDoctor(updateDoctor, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
