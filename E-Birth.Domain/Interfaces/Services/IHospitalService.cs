using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.ChildDtos;
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
    public interface IHospitalService
    {
        public Task<ApiResponse<AuthResultDto>> HospitalRegister(CreateHospital createHospital, CancellationToken cancellationToken);
        public Task<ApiResponse<GetHospitalResponse>> GetHospital(GetHospitalRequest getHospitalRequest, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteHospital(DeleteHospitalRequest deleteHospitalRequest, CancellationToken cancellationToken);


        public Task<ApiResponse<HospitalDashboard>> GetHospitalDashboard(string email, CancellationToken cancellationToken);
        public Task<ApiResponse<UserResponseForDashboard>> GetUserDetailsAsync(string NationalId, CancellationToken cancellationToken);


        public Task<ApiResponse<IEnumerable<AllDoctorSpecificDetails>>> GetAllDoctors(CancellationToken cancellationToken);
        public Task<ApiResponse<GetDoctorForReview>> GetSpecificDoctorsById(int DoctorId, CancellationToken cancellationToken);
        public Task<ApiResponse<GetDoctorForReview>> GetSpecificDoctorsByNationalId(string DoctorNationalId, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> ApproveDoctor(int DoctorId,CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteDoctor(int DoctorId,CancellationToken cancellationToken);


        public Task<ApiResponse<IEnumerable<AllParentSpecificDetails>>> GetAllParents(CancellationToken cancellationToken);

        public Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsByIdAsync(int ParentId, CancellationToken cancellationToken);
        public Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsByNationalIdAsync(string ParentNationalId, CancellationToken cancellationToken);
        public Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> AddParent(AddParentFromHospitalDto addParentFromHospitalDto, CancellationToken cancellationToken);



        public Task<ApiResponse<IEnumerable<AllChildSpecificDetail>>> GetAllChilds(CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificVaccinate>> GetSpecificChildVaccinationsForUpdate(int SpecificVaccinateId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsByIdAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsByNationalIdAsync(string NationalId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> UpdateSpecificVaccinationAsync(UpdateChildVaccinationsDto updateChildVaccinationsDto, CancellationToken cancellationToken);

        public Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildDetailsResponse>> UpdateChildProfile(UpdateChildRequest updateChild, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteChild(DeleteChildRequest deleteChildRequest, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildDetailsResponse>> AddChildAsync(CreateChildDto createChildDto, CancellationToken cancellationToken);

    }
}
