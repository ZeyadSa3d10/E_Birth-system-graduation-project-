using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(Roles = "Parent")]
    public class ParentController :ControllerBase
    {
        private readonly IChildService childService;
        private readonly IParentService parentService;

        public ParentController(IChildService childService,IParentService parentService )
        {
            this.childService = childService;
            this.parentService = parentService;
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetParentWithChilderen")]
        public async Task<IActionResult> GetParentWithChilderen([FromBody]string ParentAspNetUserId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetParentWithChilderen(ParentAspNetUserId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetChildDetailsAsync")]
        public async Task<IActionResult> GetChildDetailsAsync([FromBody]int ChildId, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildDetailsAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetChildVaccinationsAsync")]
        public async Task<IActionResult> GetChildVaccinationsAsync([FromBody]int ChildId, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildVaccinationsAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetChildMedicalHistoryAsync")]
        public async Task<IActionResult> GetChildMedicalHistoryAsync([FromBody]int ChildId, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildMedicalHistoryAsync(ChildId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetSpecificChildMedicalHistoryAsync")]
        public async Task<IActionResult> GetSpecificChildMedicalHistoryAsync([FromBody]int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await childService.GetSpecificChildMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetParentDetailsAsync")]
        public async Task<IActionResult> GetParentDetailsAsync([FromBody]int ParentId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetParentDetailsAsync(ParentId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetParentMedicalHistoryAsync([FromBody] int ParentId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetParentMedicalHistoryAsync(ParentId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("GetSpecificParentMedicalHistoryAsync")]
        public async Task<IActionResult> GetSpecificParentMedicalHistoryAsync([FromBody]int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetSpecificParentMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("UpdateParentProfile")]
        public async Task<IActionResult> UpdateParentProfile([FromBody] UpdateParent updateParent, CancellationToken cancellationToken)
        {
            var result = await parentService.UpdateParentProfile(updateParent, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}