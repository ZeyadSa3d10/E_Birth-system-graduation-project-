using E_Birth.Application.DoctorService;
using E_Birth.Application.HospitalService;
using E_Birth.Application.ParentService;
using E_Birth.Domain.ApplicationDtos.ChildDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HospitalDashboardController:ControllerBase
    {
        private readonly IChildService childService;
        private readonly IHospitalService hospitalService;

        public HospitalDashboardController(IChildService childService,
                                            IHospitalService hospitalService)
        {
            this.childService = childService;
            this.hospitalService = hospitalService;
        }
        [HttpPost("GetHospitalDashboard")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHospitalDashboard([FromBody] string email, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetHospitalDashboard(email, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetUserDetailsAsync")]
        public async Task<IActionResult> GetUserDetailsAsync([FromBody] string NationalId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetUserDetailsAsync(NationalId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetAllChilds")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllChilds(CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetAllChilds(cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("CreateChild")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateChild(CreateChildDto createChildDto, CancellationToken cancellationToken)
        {
            var result = await childService.CreateChildAsync(createChildDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("UpdateChildProfile")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateChildProfile(UpdateChildRequest updateChild, CancellationToken cancellationToken)
        {
            var result = await hospitalService.UpdateChildProfile(updateChild, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("DeleteChild")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteChild(DeleteChildRequest deleteChildRequest, CancellationToken cancellationToken)
        {
            var result = await hospitalService.DeleteChild(deleteChildRequest, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetChildDetailsByIdAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChildDetailsByIdAsync([FromBody]int ChildId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetChildDetailsByIdAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetChildByNationalId")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChildByNationalId([FromBody]string NationalID, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildByNationalId(NationalID, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetChildVaccinationsAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChildVaccinationsAsync([FromBody] int ChildId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetChildVaccinationsAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("UpdateSpecificVaccinationAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSpecificVaccinationAsync([FromBody] UpdateChildVaccinationsDto updateChildVaccinationsDto, CancellationToken cancellationToken)
        {
            var result = await hospitalService.UpdateSpecificVaccinationAsync(updateChildVaccinationsDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetChildMedicalHistoryAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetChildMedicalHistoryAsync([FromBody] int ChildId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetChildMedicalHistoryAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetSpecificChildMedicalHistoryAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSpecificChildMedicalHistoryAsync([FromBody] int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetSpecificChildMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetAllDoctors")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllDoctors(CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetAllDoctors( cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetSpecificDoctorsByNationalId")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSpecificDoctorsByNationalId([FromBody]string NationalId,CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetSpecificDoctorsByNationalId(NationalId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetSpecificDoctorsById")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSpecificDoctorsById([FromBody]int Id ,CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetSpecificDoctorsById(Id, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("DeleteDoctor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDoctor([FromBody]int DoctorId ,CancellationToken cancellationToken)
        {
            var result = await hospitalService.DeleteDoctor(DoctorId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("ApproveDoctor")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ApproveDoctor([FromBody]int DoctorId ,CancellationToken cancellationToken)
        {
            var result = await hospitalService.ApproveDoctor(DoctorId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("GetAllParents")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllParents(CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetAllParents( cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetParentDetailsByIdAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetParentDetailsByIdAsync([FromBody]int ParentId,CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetParentDetailsByIdAsync(ParentId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("GetParentDetailsByNationalIdAsync")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetParentDetailsByNationalIdAsync([FromBody]string NationalId,CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetParentDetailsByNationalIdAsync(NationalId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("AddParent")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddParent([FromBody]AddParentFromHospitalDto addParentFromHospitalDto,CancellationToken cancellationToken)
        {
            var result = await hospitalService.AddParent(addParentFromHospitalDto, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetParentMedicalHistoryAsync([FromBody] int ParentId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetParentMedicalHistoryAsync(ParentId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetSpecificParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetSpecificParentMedicalHistoryAsync([FromBody] int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await hospitalService.GetSpecificParentMedicalHistoryAsync(MedicalRecordId, cancellationToken);
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
    }
}
