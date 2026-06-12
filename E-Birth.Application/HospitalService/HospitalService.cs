using E_Birth.Application.RoleService;
using E_Birth.Application.TokenService;
using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.ChildDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Repositories;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace E_Birth.Application.HospitalService
{
    public class HospitalService : IHospitalService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHospitalRepository hospitalRepository;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IUnitofwork unitofwork;
        private readonly IRoleService roleService;
        private readonly ITokenService tokenService;
        private readonly IMemoryCache cashe;
        private readonly IDoctorReposatory doctorReposatory;
        private readonly IParentReposatory parentReposatory;
        private readonly IChildReposatory childReposatory;
        private readonly IChildService childService;
        private readonly IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory;
        private readonly IChildVaccinationRepository childVaccinationRepository;
        private readonly IDoctorAttachmentRepository doctorAttachmentRepository;
        private readonly IParentService parentService;
        private readonly IDoctorService doctorService;

        public HospitalService(UserManager<ApplicationUser> userManager,
                               IHospitalRepository hospitalRepository,
                               ApplicationDbContext applicationDbContext,
                               IUnitofwork unitofwork,
                               IRoleService roleService,
                               ITokenService tokenService,
                               IMemoryCache cashe,
                               IDoctorReposatory doctorReposatory,
                               IParentReposatory parentReposatory,
                               IChildReposatory childReposatory,
                               IChildService childService,
                               IAllOfficialVaccinationReposatory allOfficialVaccinationReposatory,
                               IChildVaccinationRepository childVaccinationRepository,
                               IDoctorAttachmentRepository doctorAttachmentRepository,
                               IParentService parentService ,
                               IDoctorService doctorService
                              )
        {
            this.userManager = userManager;
            this.hospitalRepository = hospitalRepository;
            this.applicationDbContext = applicationDbContext;
            this.unitofwork = unitofwork;
            this.roleService = roleService;
            this.tokenService = tokenService;
            this.cashe = cashe;
            this.doctorReposatory = doctorReposatory;
            this.parentReposatory = parentReposatory;
            this.childReposatory = childReposatory;
            this.childService = childService;
            this.allOfficialVaccinationReposatory = allOfficialVaccinationReposatory;
            this.childVaccinationRepository = childVaccinationRepository;
            this.doctorAttachmentRepository = doctorAttachmentRepository;
            this.parentService = parentService;
            this.doctorService = doctorService;
        }

        public Task<ApiResponse<ChildDetailsResponse>> AddChildAsync(CreateChildDto createChildDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<string>> AddParent(AddParentFromHospitalDto addParentFromHospitalDto, CancellationToken cancellationToken)
        {
            return parentService.AddParent(addParentFromHospitalDto, cancellationToken);
        }

        public async Task<ApiResponse<string>> ApproveDoctor(int DoctorId, CancellationToken cancellationToken)
        {
            var Doctor = await doctorReposatory.GetByIdAsync(DoctorId, cancellationToken);
            if (Doctor == null || Doctor.IsDeleted == true)
                return ApiResponse<string>.ErrorResponse("Doctor Not Found", 404);
            Doctor.IsApproved = true;
            var result = await doctorReposatory.Update(Doctor, cancellationToken);
            if (result == null)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            var ResultOfSaving = await unitofwork.SaveChangesAsync(cancellationToken);
            if (ResultOfSaving <= 0)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            return ApiResponse<string>.Success("Doctor Approved successfully");
        }

        public async Task<ApiResponse<string>> DeleteChild(DeleteChildRequest deleteChildRequest, CancellationToken cancellationToken)
        {
           return await childService.DeleteChild(deleteChildRequest, cancellationToken);
        }

        public async Task<ApiResponse<string>> DeleteDoctor(int DoctorId, CancellationToken cancellationToken)
        {
            var Doctor =await doctorReposatory.GetByIdAsync(DoctorId, cancellationToken);
            if (Doctor == null )
                return ApiResponse<string>.ErrorResponse("Doctor Not Found", 404);
            Doctor.IsDeleted = true;
            var result = await doctorReposatory.Update(Doctor, cancellationToken);
            if (result == null)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            var ResultOfSaving = await unitofwork.SaveChangesAsync(cancellationToken);
            if (ResultOfSaving <= 0)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            return ApiResponse<string>.Success("Doctor deleted successfully");
        }

        public async Task<ApiResponse<string>> DeleteHospital(DeleteHospitalRequest deleteHospitalRequest, CancellationToken cancellationToken)
        {
            if (deleteHospitalRequest.Email is null)
            {
                return ApiResponse<string>.ErrorResponse("Email is required");
            }
            var hospital = await hospitalRepository.GetByEmailAsync(deleteHospitalRequest.Email, cancellationToken);
            if (hospital is null || hospital.IsDeleted == true)
            {
                return ApiResponse<string>.ErrorResponse("Hospital not found", 404);
            }
            hospital.IsDeleted = true;
            await hospitalRepository.Update(hospital, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
            {
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            }
            var user = await userManager.FindByEmailAsync(deleteHospitalRequest.Email);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            cashe.Remove(deleteHospitalRequest.Email);
            return ApiResponse<string>.Success("Hospital deleted successfully");
        }

        public async Task<ApiResponse<IEnumerable<AllChildSpecificDetail>>> GetAllChilds(CancellationToken cancellationToken)
        {
           var result = await childService.GetAllChilds(cancellationToken);
            return result;
        }

        public async Task<ApiResponse<IEnumerable<AllDoctorSpecificDetails>>> GetAllDoctors(CancellationToken cancellationToken)
        {
            var doctors = await doctorReposatory.GetAllAsync(D=>D.IsDeleted==false,cancellationToken);
            if (!doctors.Any())
                return ApiResponse<IEnumerable<AllDoctorSpecificDetails>>.ErrorResponse("Doctors Not Found");
            var NotApproveDoctors = doctors.Where(A=>A.IsApproved==false).ToArray();
            var ApproveDoctors = doctors.Where(A=>A.IsApproved==true).ToArray();
            var allDoctorSpecificDetails = new List<AllDoctorSpecificDetails>();
            foreach(var doc in NotApproveDoctors)
            {
                var newDoctor = new AllDoctorSpecificDetails()
                {
                    Id=doc.Id,
                    Name=doc.FullName,
                    NationalId=doc.NationalId,
                    email=doc.Email,
                    PhoneNumber=doc.PhoneNumber,
                    statusOfDoctor="غير معتمد"
                };
                allDoctorSpecificDetails.Add(newDoctor);
            }
            foreach(var doc in ApproveDoctors)
            {
                var newDoctor = new AllDoctorSpecificDetails()
                {
                    Id=doc.Id,
                    Name=doc.FullName,
                    NationalId=doc.NationalId,
                    email=doc.Email,
                    PhoneNumber=doc.PhoneNumber,
                    statusOfDoctor="معتمد"
                };
                allDoctorSpecificDetails.Add(newDoctor);
            }
            return ApiResponse<IEnumerable<AllDoctorSpecificDetails>>.Success(allDoctorSpecificDetails);
        }

        public async Task<ApiResponse<IEnumerable<AllParentSpecificDetails>>> GetAllParents(CancellationToken cancellationToken)
        {
           var parents = await parentReposatory.GetAllAsync(P=>P.IsDeleted ==false ,cancellationToken);
            if (!parents.Any())
                return ApiResponse<IEnumerable<AllParentSpecificDetails>>.ErrorResponse("Parents Not Found");
            var allParentSpecificDetails = parents.Select(p => new AllParentSpecificDetails
            {
                Id = p.Id,
                Name = p.FullName,
                NationalId = p.NationalId,
                email = p.Email,
                PhoneNumber = p.PhoneNumber
            }).ToList();
            return ApiResponse<IEnumerable<AllParentSpecificDetails>>.Success(allParentSpecificDetails);
        }

        public async Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsByIdAsync(int ChildId, CancellationToken cancellationToken)
        {
            return await childService.GetChildDetailsAsync(ChildId, cancellationToken);
        }

        public async Task<ApiResponse<ChildDetailsResponse>> GetChildDetailsByNationalIdAsync(string NationalId, CancellationToken cancellationToken)
        {
           return await childService.GetChildByNationalId(NationalId, cancellationToken);
        }

        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken)
        {
            return await childService.GetChildMedicalHistoryAsync(ChildId, cancellationToken);
        }

        public async Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken)
        {
            return await childService.GetChildVaccinationsAsync(ChildId, cancellationToken);
        }

        public async Task<ApiResponse<GetHospitalResponse>> GetHospital(GetHospitalRequest getHospitalRequest, CancellationToken cancellationToken)
        {
            var cacheKey = getHospitalRequest.Email;
            if (cashe.TryGetValue(cacheKey, out GetHospitalResponse cachedHospital))
            {
                Console.WriteLine("Hospital retrieved from cache.");
                return ApiResponse<GetHospitalResponse>.Success(cachedHospital);
            }
            if (getHospitalRequest.Email is null)
            {
                return ApiResponse<GetHospitalResponse>.ErrorResponse("Email is required");
            }
            var hospital = await hospitalRepository.GetByEmailAsync(getHospitalRequest.Email, cancellationToken);
            if (hospital is null || hospital.IsDeleted == true)
            {
                return ApiResponse<GetHospitalResponse>.ErrorResponse("Hospital not found", 404);
            }
            if (hospital.IsApproved == false)
            {
                return ApiResponse<GetHospitalResponse>.ErrorResponse("Your account is under review by admin", 403);
            }
            var response = new GetHospitalResponse
            {
                Id=hospital.Id,
                Name = hospital.Name,
                PhoneNumber = hospital.PhoneNumber,
                Email = hospital.Email,
                Governorate = hospital.Governorate.ToString()
            };
            cashe.Set(cacheKey, response, new MemoryCacheEntryOptions
            {
                Size = 1,
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });
            return ApiResponse<GetHospitalResponse>.Success(response);
        }

        public async Task<ApiResponse<HospitalDashboard>> GetHospitalDashboard(string email, CancellationToken cancellationToken)
        {
            var hospital = await hospitalRepository.GetByEmailAsync(email, cancellationToken);
            if (hospital == null)
            {
                return ApiResponse<HospitalDashboard>.ErrorResponse("Hospital not found", 404);
            }
            var Totaldoctors = await doctorReposatory.CountAsync(cancellationToken);
            var WaitingDoctors = await doctorReposatory.CountAsync(d => d.IsApproved == false, cancellationToken);
            var TotalParents = await parentReposatory.CountAsync(cancellationToken);
            var TotalChilds = await childReposatory.CountAsync(cancellationToken);
            var ChildWithLateVaccinateCount = await childService.GetLateVaccinationsCountAsync(cancellationToken);
            return ApiResponse<HospitalDashboard>.Success(new HospitalDashboard
            {
                HospitalName = hospital.Name,
                hospitalId= hospital.Id,
                TotalDoctors = Totaldoctors,
                WaitingDoctors = WaitingDoctors,
                TotalParents = TotalParents,
                TotalChilds = TotalChilds,
                ChildWithLateVaccinate = ChildWithLateVaccinateCount.Data
            });
        }

        public async Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsByIdAsync(int ParentId, CancellationToken cancellationToken)
        {
            return await parentService.GetParentDetailsAsync(ParentId, cancellationToken);
        }

        public async Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsByNationalIdAsync(string ParentNationalId, CancellationToken cancellationToken)
        {
            var parent =await parentReposatory.GetByNationalIdAsync(ParentNationalId, cancellationToken);
            if (parent == null || parent.IsDeleted == true)
                return ApiResponse<ParentDetailsResponse>.ErrorResponse("Parent Not Found", 404);
            return await GetParentDetailsByIdAsync(parent.Id, cancellationToken);
        }

        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId, CancellationToken cancellationToken)
        {
           return await parentService.GetParentMedicalHistoryAsync(ParentId, cancellationToken);
        }

        public Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            return childService.GetSpecificChildMedicalHistoryAsync(MedicalRecordId, cancellationToken);
        }

        public Task<ApiResponse<SpecificVaccinate>> GetSpecificChildVaccinationsForUpdate(int SpecificVaccinateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<GetDoctorForReview>> GetSpecificDoctorsById(int DoctorId, CancellationToken cancellationToken)
        {
            var Doctor = await doctorReposatory.GetByIdAsync(DoctorId, cancellationToken);
            if (Doctor == null || Doctor.IsDeleted == true)
                return ApiResponse<GetDoctorForReview>.ErrorResponse("Doctor Not Found", 404);
            var DoctorAttachmentUrl =await doctorAttachmentRepository.GetByDoctorIdAsync(DoctorId, cancellationToken);
            var status = Doctor.IsApproved == true ? "معتمد" : "غير معتمد";
            GetDoctorForReview getDoctorForReview = new GetDoctorForReview()
            {
                BirthDate=Doctor.BirthDate,
                 AttachmentUrl=DoctorAttachmentUrl,
                 Status=status,
                 BloodType=Doctor.BloodType.ToString(),
                 City=Doctor.City,
                 Email=Doctor.Email,
                 FullName=Doctor.FullName,
                 Gender =Doctor.Gender.ToString(),
                 Governorate= Doctor.Governorate.ToString(),
                 Id=Doctor.Id,
                 NationalId=Doctor.NationalId,
                 PhoneNumber=Doctor.PhoneNumber,
                 Village=Doctor.Village,
            };
            return ApiResponse<GetDoctorForReview>.Success(getDoctorForReview);
        }

        public async Task<ApiResponse<GetDoctorForReview>> GetSpecificDoctorsByNationalId(string DoctorNationalId, CancellationToken cancellationToken)
        {
            var doctor = await doctorReposatory.GetByNationalIdAsync(DoctorNationalId, cancellationToken);
            if (doctor == null)
                return ApiResponse<GetDoctorForReview>.ErrorResponse("Doctor Not Found", 404);
            return await GetSpecificDoctorsById(doctor.Id, cancellationToken);
        }

        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            return await parentService.GetSpecificParentMedicalHistoryAsync(MedicalRecordId, cancellationToken);
        }

        public Task<ApiResponse<UserResponseForDashboard>> GetUserDetailsAsync(string NationalId, CancellationToken cancellationToken)
        {
            return doctorService.GetUserDetailsAsync(NationalId, cancellationToken);
        }

        public async Task<ApiResponse<AuthResultDto>> HospitalRegister(CreateHospital createHospital, CancellationToken cancellationToken)
        {

            var errors = new List<string>();
            if (createHospital.Passworded != createHospital.ConfirmPassworded)
                errors.Add("Passwords do not match");
            var ExistHospital = await userManager.FindByEmailAsync(createHospital.Email);
            if (ExistHospital != null)
                errors.Add("Email already exists");
            var CheckForHospitalInDB = await hospitalRepository.GetByEmailAsync(createHospital.Email, cancellationToken);
            if (CheckForHospitalInDB != null)
                errors.Add("Email already exists");
            if (errors.Any())
                return ApiResponse<AuthResultDto>.ErrorResponse(errors);
            var NewHospital = new Hospital
            {
                Email = createHospital.Email,
                Latitude = createHospital.Latitude,
                Longitude = createHospital.Longitude,
                Name = createHospital.Name,
                PhoneNumber = createHospital.PhoneNumber,
                Governorate = createHospital.Governorate,
                IsDeleted = false,
                IsApproved = false,
            };
            using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await hospitalRepository.AddAsync(NewHospital, cancellationToken);
                var result = await unitofwork.SaveChangesAsync(cancellationToken);

                if (result <= 0)
                    return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new Hospital");


                var user = new ApplicationUser
                {
                    FullName = createHospital.Name,
                    UserName = createHospital.Email,
                    Email = createHospital.Email,
                    PhoneNumber = createHospital.PhoneNumber,
                };

                var newUser = await userManager.CreateAsync(user, createHospital.Passworded);
                if (!newUser.Succeeded)
                {
                    return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new Hospital");
                }

                await roleService.EnsureRoleExistsAsync("Hospital");
                await roleService.AssignRoleAsync(user, "Hospital");


                await transaction.CommitAsync(cancellationToken);

                var token = await tokenService.GenerateToken(user, cancellationToken);
                var response = new AuthResultDto
                {
                    Token = token
                };

                return ApiResponse<AuthResultDto>.Success(response, 201);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return ApiResponse<AuthResultDto>.ErrorResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<ChildDetailsResponse>> UpdateChildProfile(UpdateChildRequest updateChild, CancellationToken cancellationToken)
        {
           return await childService.UpdateChildProfile(updateChild, cancellationToken);
        }

       

        public async Task<ApiResponse<UpdateHospitalResponse>> UpdateHospital(UpdateHospitalRequest updateHospitalRequest, CancellationToken cancellationToken)
        {
            if (updateHospitalRequest.Email is null)
            {
                return ApiResponse<UpdateHospitalResponse>.ErrorResponse("Email is required");
            }
            var hospital = await hospitalRepository.GetByEmailAsync(updateHospitalRequest.Email, cancellationToken);
            if (hospital is null || hospital.IsDeleted == true)
            {
                return ApiResponse<UpdateHospitalResponse>.ErrorResponse("Hospital not found", 404);
            }
            if (hospital.IsApproved == false)
            {
                return ApiResponse<UpdateHospitalResponse>.ErrorResponse("Your account is under review by admin", 403);
            }

            hospital.Name = updateHospitalRequest.Name!;
            hospital.Latitude = updateHospitalRequest.Latitude!;
            hospital.Longitude = updateHospitalRequest.Longitude!;
            hospital.PhoneNumber = updateHospitalRequest.PhoneNumber!;
            hospital.Email = updateHospitalRequest.Email!;
            hospital.Governorate = updateHospitalRequest.Governorate;
            hospital.IsApproved = false;
            hospital.IsDeleted = false;
            await hospitalRepository.Update(hospital, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
            {
                return ApiResponse<UpdateHospitalResponse>.ErrorResponse("Error Occure While Saving Entity In Database");
            }
            UpdateHospitalResponse updateHospitalResponse = new UpdateHospitalResponse()
            {
                Email = hospital.Email,
                Governorate = hospital.Governorate.ToString(),
                Name = hospital.Name,
                Latitude = hospital.Latitude,
                Longitude = hospital.Longitude,
                PhoneNumber = hospital.PhoneNumber,
            };
            cashe.Remove(updateHospitalRequest.Email);
            return ApiResponse<UpdateHospitalResponse>.Success(updateHospitalResponse);
        }

        public async Task<ApiResponse<string>> UpdateSpecificVaccinationAsync(UpdateChildVaccinationsDto updateChildVaccinationsDto, CancellationToken cancellationToken)
        {
            var CheckForChild = await childReposatory.GetByIdAsync(updateChildVaccinationsDto.ChildId, cancellationToken);
            if (CheckForChild is null || CheckForChild.IsDeleted ==true)
               return ApiResponse<string>.ErrorResponse("Child not found", 404);
            var CheckForVaccination = await allOfficialVaccinationReposatory.GetByIdAsync(updateChildVaccinationsDto.SpecificVaccinationId, cancellationToken);
            if (CheckForVaccination is null)
                return ApiResponse<string>.ErrorResponse("Vaccination not found", 404);
            var CheckForHospital = await hospitalRepository.GetByIdAsync(updateChildVaccinationsDto.HospitalId, cancellationToken);
            if (CheckForHospital is null)
                return ApiResponse<string>.ErrorResponse("Hospital not found", 404);
            ChildVaccination childVaccination = new ChildVaccination
            {
                ChildId = updateChildVaccinationsDto.ChildId,
                AllOfficialVaccinationId = updateChildVaccinationsDto.SpecificVaccinationId,
                DateGiven = DateTime.UtcNow,
                HospitalId = updateChildVaccinationsDto.HospitalId,
            };
            var UpdateChildVaccinate = await childVaccinationRepository.AddAsync(childVaccination, cancellationToken);
            if (UpdateChildVaccinate is null)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<string>.ErrorResponse("Error Occure While Saving Entity In Database");
            return ApiResponse<string>.Success("Child vaccination updated successfully");
        }
    }
}
