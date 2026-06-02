using E_Birth.Domain.ApplicationDtos.ChildDtos;
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
    public interface IChildService
    {

        public Task<ApiResponse<ChildResponseDto>> CreateChildAsync(CreateChildDto createChild, CancellationToken cancellationToken);

        public Task<ApiResponse<int>> GetLateVaccinationsCountAsync(CancellationToken cancellationToken);
        public Task<ApiResponse<IEnumerable<ChildResponseDto>>> GetChildrenWithLateVaccinationsAsync(CancellationToken cancellationToken);

        public Task<ApiResponse<IEnumerable<AllChildSpecificDetail>>> GetAllChilds(CancellationToken cancellationToken);

        public Task<ApiResponse<ChildDetailsResponse>> GetChildByNationalId(string NationalId,CancellationToken cancellationToken);
        public Task<ApiResponse<IEnumerable<ChildResponseDto>?>> GetChildByParentNationalId(string NationalId,CancellationToken cancellationToken);
        
        
        public Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<ChildDetailsResponse>> UpdateChildProfile(UpdateChildRequest updateChild, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteChild(DeleteChildRequest deleteChildRequest, CancellationToken cancellationToken);
    }
}
