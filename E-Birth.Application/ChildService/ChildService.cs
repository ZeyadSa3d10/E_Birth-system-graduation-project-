using E_Birth.Domain.ApplicationDtos.ChildDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Enums;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.Repositories;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Application.ChildService
{
    public class ChildService : IChildService
    {
        private readonly IChildVaccinationRepository childVaccinationRepository;
        private readonly IParentReposatory parentReposatory;
        private readonly IChildReposatory childReposatory;
        private readonly IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory;
        private readonly IUserMedicalRecordRepository userMedicalRecordRepository;
        private readonly IDoctorReposatory doctorReposatory;
        private readonly IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository;
        private readonly IUnitofwork unitofwork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRoleService roleService;

        public ChildService(IChildVaccinationRepository childVaccinationRepository,
                            IParentReposatory parentReposatory,
                            IChildReposatory childReposatory,
                            IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory,
                            IUserMedicalRecordRepository userMedicalRecordRepository,
                            IDoctorReposatory doctorReposatory,
                            IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository,
                            IUnitofwork unitofwork,
                            UserManager<ApplicationUser> userManager,
                            IRoleService roleService
                                                            )
        {
            this.childVaccinationRepository = childVaccinationRepository;
            this.parentReposatory = parentReposatory;
            this.childReposatory = childReposatory;
            this.allOfficialVaccinationReposatory = allOfficialVaccinationReposatory;
            this.userMedicalRecordRepository = userMedicalRecordRepository;
            this.doctorReposatory = doctorReposatory;
            this.userMedcalRecordImagesRepository = userMedcalRecordImagesRepository;
            this.unitofwork = unitofwork;
            this.userManager = userManager;
            this.roleService = roleService;
        }

        #region Childeren For Dashboard
        public async Task<ApiResponse<IEnumerable<ChildResponseDto>>> GetChildrenWithLateVaccinationsAsync(CancellationToken cancellationToken)
        {
            var children = await childVaccinationRepository.GetChildrenWithLateVaccinationsAsync(cancellationToken);
            if (children == null)
                return ApiResponse<IEnumerable<ChildResponseDto>>.ErrorResponse("Not Found", 204);
            var ChildrenResponse = new List<ChildResponseDto>();
            foreach (var child in children)
            {
                var Parent = await parentReposatory.GetByIdAsync(child.ParentId, cancellationToken);
                if (Parent == null)
                    return ApiResponse<IEnumerable<ChildResponseDto>>.ErrorResponse("Parent Not Found", 404);
                var childResponse = new ChildResponseDto
                {
                    Id = child.Id,
                    FullName = child.FullName,
                    NationalId = child.NationalId,
                    ParentEmail = Parent.Email,
                    ParentPhoneNumber = Parent.PhoneNumber
                };
                ChildrenResponse.Add(childResponse);
            }
            return ApiResponse<IEnumerable<ChildResponseDto>>.Success(ChildrenResponse);

        }
        public async Task<ApiResponse<int>> GetLateVaccinationsCountAsync(CancellationToken cancellationToken)
        {
            var count = await childVaccinationRepository.GetLateVaccinationsCountAsync(cancellationToken);
            if (count < 0)
            {
                return ApiResponse<int>.ErrorResponse("Failed to retrieve late vaccinations count");
            }

            return ApiResponse<int>.Success(count);
        } 
        #endregion






        public async Task<ApiResponse<ChildDetailsResponse>> GetChildByNationalId(string NationalId,CancellationToken cancellationToken)
        {
            var child =await childReposatory.GetByNationalIdAsync(NationalId, cancellationToken);
            if(child == null)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Not Found", 204);
            return await GetChildDetailsAsync(child.Id, cancellationToken);
        }

        public Task<ApiResponse<IEnumerable<ChildResponseDto>?>> GetChildByParentNationalId(string NationalId,CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


       
        private AgeWithMonthsAndYears CalculateAge(DateOnly birthDate)
        {
            var today = DateTime.Today;

            int years = today.Year - birthDate.Year;
            int months = today.Month - birthDate.Month;

            // لو لسه موصلش ليوم الميلاد
            if (today.Day < birthDate.Day)
            {
                months--;
            }

            // لو الشهور بالسالب
            if (months < 0)
            {
                years--;
                months += 12;
            }

            return new AgeWithMonthsAndYears
            {
                Years = years,
                Months = months
            };
        }

        public async Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsAsync(int ChildId, CancellationToken cancellationToken)
        {
            var child = await childReposatory.GetByIdAsync(ChildId, cancellationToken);
            if (child == null||child.IsDeleted==true)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Child Not Found", 404);
            var parent = await parentReposatory.GetByIdAsync(child.ParentId, cancellationToken);
            if (parent == null)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Parent Not Found", 404);
            ChildDetailsResponse childDetails = new ChildDetailsResponse()
            {
                ChildId = child.Id,
                ChildFullName = child.FullName,
                ChildNationalId = child.NationalId,
                BirthDate = child.BirthDate,
                Gender = child.Gender.ToString(),
                BloodType = child.BloodType.ToString(),
                Governorate = child.Governorate.ToString(),
                Village = child.Village,
                City = child.City,

                ParentEmail = parent.Email,
                ParentFullName = parent.FullName,
                ParentNationalId = parent.NationalId,
                ParentPhoneNumber = parent.PhoneNumber,
            };
            return ApiResponse<ChildDetailsResponse>.Success(childDetails);
        }
        public async Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken)
        {
            var child = await childReposatory.GetByIdAsync(ChildId, cancellationToken);
            if (child == null)
                return ApiResponse<ChildVaccinationsResponse>.ErrorResponse("Child Not Found", 404);

            var age = CalculateAge(child.BirthDate);
            var ageInMonths = age.Years * 12 + age.Months;

            var officialVaccinations = await allOfficialVaccinationReposatory.GetAllAsync(cancellationToken);
            if (officialVaccinations == null || !officialVaccinations.Any())
                return ApiResponse<ChildVaccinationsResponse>.ErrorResponse("No Official Vaccinations Found", 404);

            var childVaccinations = await childVaccinationRepository.GetAllAsync(c => c.ChildId == child.Id, cancellationToken);

            var vaccinationsList = new List<SpecificVaccinate>();

            foreach (var official in officialVaccinations)
            {
                var taken = childVaccinations
                    .FirstOrDefault(c => c.AllOfficialVaccinationId == official.Id);

                var officialDate = child.BirthDate.AddMonths(official.AgeInMonths);

                StatusOfChildVaccination status;

                if (taken != null)
                {
                    status = StatusOfChildVaccination.Completed;
                }
                else if (official.AgeInMonths > ageInMonths)
                {
                    status = StatusOfChildVaccination.Upcoming;
                }
                else
                {
                    status = StatusOfChildVaccination.Late;
                }

                vaccinationsList.Add(new SpecificVaccinate
                {
                    IdForSpecificVaccinateFromAll= official.Id,
                    ChildVaccinationsName = official.VaccinationType,
                    OfficialDate = officialDate,
                    TakenDate = taken?.DateGiven,
                    Status = status.ToString()
                });
            }

            var response = new ChildVaccinationsResponse
            {
                ChildId = child.Id,
                Gender = child.Gender.ToString(),
                ChildAgeInMonths = ageInMonths,
                ChildAgeInYears = age.Years,
                Vaccinations = vaccinationsList
            };

            return ApiResponse<ChildVaccinationsResponse>.Success(response, 200);
        }
        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken)
        {
            var child = await childReposatory.GetByIdAsync(ChildId, cancellationToken);
            if (child == null || child.IsDeleted ==true)
                return ApiResponse<UserMedicalHistoryResponse>.ErrorResponse("Child Not Found", 404);
            var age = CalculateAge(child.BirthDate);
            var userMedicalRecords = await userMedicalRecordRepository.GetAllAsync(m => m.ChildId == ChildId, cancellationToken);
            
            var userDetails=new List<UserDetails>();
           
            
            foreach (var record in userMedicalRecords)
            {
                var doctor = await doctorReposatory.GetByIdAsync(record.DoctorId, cancellationToken);
                if (doctor == null)
                    return ApiResponse<UserMedicalHistoryResponse>.ErrorResponse("Doctor Not Found", 404);
                userDetails.Add(new UserDetails
                {
                    MedicalRecordId = record.Id,
                    DoctorName = doctor.FullName,
                    GivenAt = record.CreatedAt,
                    Description = record.Description,
                    Medicine = record.Medicine
                });
            }

            var response = new UserMedicalHistoryResponse
            {
                UserId = child.Id,
                UserName=child.FullName,
                UserAgeInMonths=age.Months,
                UserAgeInYears=age.Years,
                Gender= child.Gender.ToString(),
                MedicalHistory=userDetails
            };
            return ApiResponse<UserMedicalHistoryResponse>.Success(response, 200);
        }

        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            var MedicalRecord = await userMedicalRecordRepository.GetByIdAsync(MedicalRecordId, cancellationToken);
            if (MedicalRecord == null)
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Medical Record Not Found", 404);
            var doctor = await doctorReposatory.GetByIdAsync(MedicalRecord.DoctorId, cancellationToken);
            if (doctor == null)
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Doctor Not Found", 404);
           
            int childId =int.Parse(MedicalRecord.ChildId.ToString());
            var userMedcalRecordImages=await userMedcalRecordImagesRepository.GetAllAsync(i => i.UserMedicalRecordId == MedicalRecordId, cancellationToken);
            
            var response = new SpecificUserMedicalHistoryResponse
            {
                UserId =childId,
                MedicalRecordId = MedicalRecord.Id,
                DoctorName = doctor.FullName,
                GivenAt = MedicalRecord.CreatedAt,
                Description = MedicalRecord.Description,
                Medicine = MedicalRecord.Medicine,
                MedicalRecordImages= userMedcalRecordImages.Select(i => i.ImagePath).ToList()
            };
            return ApiResponse<SpecificUserMedicalHistoryResponse>.Success(response, 200);
        }

        public async Task<ApiResponse<ChildDetailsResponse>> UpdateChildProfile(UpdateChildRequest updateChild, CancellationToken cancellationToken)
        {
            var Child = await childReposatory.GetByIdAsync(updateChild.Id, cancellationToken);
            if (Child == null || Child.NationalId !=updateChild.NationalId)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Child Not Found", 404);
            var CheckForParent = await parentReposatory.GetByNationalIdAsync(updateChild.ParentNationalId, cancellationToken);
            if (CheckForParent == null)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Parent Not Found", 404);
            Child.BirthDate = updateChild.BirthDate;
            Child.FullName = updateChild.FullName;
            Child.Gender = updateChild.Gender;
            Child.Village = updateChild.Village;
            Child.City = updateChild.City;
            Child.Governorate = updateChild.Governorate;
            Child.BloodType = updateChild.BloodType;
            Child.CreateAt = DateTime.UtcNow;

            await childReposatory.Update(Child, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<ChildDetailsResponse>.ErrorResponse("Error occurred while updating Child");
            ChildDetailsResponse childDetailsResponse = new ChildDetailsResponse()
            {
                ChildId = Child.Id,
                ChildFullName = Child.FullName,
                ChildNationalId = Child.NationalId,
                BirthDate = Child.BirthDate,
                Gender = Child.Gender.ToString(),
                BloodType = Child.BloodType.ToString(),
                Governorate = Child.Governorate.ToString(),
                Village = Child.Village,
                City = Child.City,
                ParentNationalId= updateChild.ParentNationalId,
                ParentEmail=CheckForParent.Email,
                ParentFullName=CheckForParent.FullName,
                ParentPhoneNumber=CheckForParent.PhoneNumber
            };
            return ApiResponse<ChildDetailsResponse>.Success(childDetailsResponse);
        }

        public async Task<ApiResponse<string>> DeleteChild(DeleteChildRequest deleteChildRequest, CancellationToken cancellationToken)
        {
            var Child = await childReposatory.GetByIdAsync(deleteChildRequest.Id, cancellationToken);
            if (Child == null)
                return ApiResponse<string>.ErrorResponse("Child Not Found", 404);
            Child.IsDeleted = true;
            await childReposatory.Update(Child, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<string>.ErrorResponse("Error occurred while deleting Child");
            return ApiResponse<string>.Success("Child deleted successfully");
        }

        public async Task<ApiResponse<ChildResponseDto>> CreateChildAsync(CreateChildDto createChild, CancellationToken cancellationToken)
        {
            var CheckParent =await parentReposatory.GetByNationalIdAsync(createChild.ParentNationalId, cancellationToken);
            if (CheckParent == null)
                return ApiResponse<ChildResponseDto>.ErrorResponse("Parent Not Found", 404);
           var existingChild = await childReposatory.GetByNationalIdAsync(createChild.NationalId, cancellationToken);
            if (existingChild != null)
                return ApiResponse<ChildResponseDto>.ErrorResponse("Child with the same National ID already exists", 400);
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.NationalId == createChild.NationalId);
            if (user != null)
                return ApiResponse<ChildResponseDto>.ErrorResponse("A user with the same National ID already exists", 400);
            var NewUser = new ApplicationUser()
            {
                NationalId= createChild.NationalId,
                FullName= createChild.FullName,
                UserName= createChild.NationalId,
                Email=CheckParent.Email
            };
            var createUserResult = await userManager.CreateAsync(NewUser,"EBirthRemote1#");
            if (!createUserResult.Succeeded)
                return ApiResponse<ChildResponseDto>.ErrorResponse("Error occurred while creating user account for the child", 500);
           await roleService.AssignRoleAsync(NewUser, "Child");
            var child = new Child()
            {
                FullName= createChild.FullName,
                NationalId= createChild.NationalId,
                BirthDate= createChild.DateOfBirth,
                Gender= createChild.Gender,
                BloodType= createChild.BloodType,
                Governorate= createChild.Governorate,
                City= createChild.City,
                Village= createChild.Village,
                ParentId= CheckParent.Id,
            };
            var createdChild = await childReposatory.AddAsync(child, cancellationToken);
            if (createdChild == null)
                return ApiResponse<ChildResponseDto>.ErrorResponse("Error occurred while creating Child", 500);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<ChildResponseDto>.ErrorResponse("Error occurred while creating Child", 500);
            return ApiResponse<ChildResponseDto>.Success(new ChildResponseDto
            {
                Id = createdChild.Id,
                FullName = createdChild.FullName,
                NationalId = createdChild.NationalId,
                ParentEmail = CheckParent.Email,
                ParentPhoneNumber = CheckParent.PhoneNumber
            }, 201);
        }

        public async Task<ApiResponse<IEnumerable<AllChildSpecificDetail>>> GetAllChilds(CancellationToken cancellationToken)
        {
            var Childs = await childReposatory.GetAllAsync(D=>D.IsDeleted==false,cancellationToken);
            if (Childs == null)
                return ApiResponse<IEnumerable<AllChildSpecificDetail>>.ErrorResponse("Childs Not Found", 404);
           var response = new List<AllChildSpecificDetail>();
            foreach (var child in Childs)
            {
                var parent = await parentReposatory.GetByIdAsync(child.ParentId, cancellationToken);
                if (parent == null)
                    return ApiResponse<IEnumerable<AllChildSpecificDetail>>.ErrorResponse("Parent Not Found", 404);
                var NewChild = new AllChildSpecificDetail
                {
                    Id=child.Id,
                    Name=child.FullName,
                    NationalId=child.NationalId,
                    PhoneNumber=parent.PhoneNumber,
                };
                response.Add(NewChild);
            }
            return ApiResponse<IEnumerable<AllChildSpecificDetail>>.Success(response, 200);
        }
    }   
}
