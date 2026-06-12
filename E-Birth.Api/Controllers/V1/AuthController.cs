using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController :ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("UserLogin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UserLogin(UserLogin userLogin, CancellationToken cancellationToken)
        {
            var result = await authService.UserLogin(userLogin, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("CreateParent")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateParent(CreateParent createParent,CancellationToken cancellationToken)
        {
            var result = await authService.ParentRegister(createParent,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("CreateHospital")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHospital(CreateHospital createHospital,CancellationToken cancellationToken)
        {
            var result = await authService.HospitalRegister(createHospital,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("CreateDoctor")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateDoctor(CreateDoctor createDoctor,CancellationToken cancellationToken)
        {
            var result = await authService.DoctorRegister(createDoctor,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpPost("ForgetPassword")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgetPassword([FromBody]string email,CancellationToken cancellationToken)
        {
            var result = await authService.ForgetPassword(email,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("IsvalidOtp")]

        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsvalidOtp(SendOtp sendOtp,CancellationToken cancellationToken)
        {
            var result = await authService.IsvalidOtp(sendOtp,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("ResendOtp")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendOtp([FromBody] string email,CancellationToken cancellationToken)
        {
            var result = await authService.ResendOtp(email,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("ResetPassword")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ApplyNewPassword applyNewPassword,CancellationToken cancellationToken)
        {
            var result = await authService.ResetPasswordAsync(applyNewPassword,cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}
