using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Models.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Services
{
    public interface IDoctorService
    {
        public Task<ApiResponse<AuthResultDto>> DoctorRegister(CreateDoctor createDoctor, CancellationToken cancellationToken);
        public Task<ApiResponse<GetDoctorResponse>> GetDoctor(int DoctorId, CancellationToken cancellationToken);
        public Task<ApiResponse<UpdateDoctorResponse>> UpdateDoctor(UpdateDoctor updateDoctor, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteDoctor(DeleteDoctor deleteDoctor, CancellationToken cancellationToken);

        public Task<ApiResponse<GetDoctorForDashboardResponse>> GetDoctorForDashboard(string DoctorAspNetUserId, CancellationToken cancellationToken);
        public Task<ApiResponse<UserResponseForDashboard>> GetUserDetailsAsync(string NationalId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> AddChildMedicalRecord(AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> AddParentMedicalRecord(AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken);
    }
}
