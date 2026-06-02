using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.ChildDtos;
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
    public interface IParentService
    {
        public Task<ApiResponse<AuthResultDto>> ParentRegister(CreateParent createParent, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> AddParent(AddParentFromHospitalDto addParentFromHospitalDto, CancellationToken cancellationToken);



        public Task<ApiResponse<IEnumerable<ParentWithChildResponse>>> GetParentWithChilderen(string ParentAspNetUserId, CancellationToken cancellationToken);
      

        
        public Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsAsync(int ParentId,CancellationToken cancellationToken);
        public Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId,CancellationToken cancellationToken);
        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken);
        public Task<ApiResponse<ParentDetailsResponse>> UpdateParentProfile(UpdateParent updateParent, CancellationToken cancellationToken);
        public Task<ApiResponse<string>> DeleteParent(DeleteParentRequest deleteParentRequest, CancellationToken cancellationToken);
    }
}
