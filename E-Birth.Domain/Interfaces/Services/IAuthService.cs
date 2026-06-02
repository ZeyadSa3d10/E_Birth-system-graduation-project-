using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<ApiResponse<AuthResultDto>> UserLogin(UserLogin userLogin, CancellationToken cancellationToken);
        public Task<ApiResponse<AuthResultDto>> DoctorRegister(CreateDoctor createDoctor, CancellationToken cancellationToken);
        public Task<ApiResponse<AuthResultDto>> ParentRegister(CreateParent createParent, CancellationToken cancellationToken);
        public Task<ApiResponse<AuthResultDto>> HospitalRegister(CreateHospital createHospital, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> ForgetPassword(string email, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> ResendOtp(string email, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> IsvalidOtp(SendOtp sendOtp, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> ResetPasswordAsync(ApplyNewPassword request,CancellationToken cancellationToken);
    }
}
