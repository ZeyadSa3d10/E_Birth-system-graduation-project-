
using E_Birth.Application.RoleService;
using E_Birth.Application.TokenService;
using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Reposatories;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Birth.Application.DoctorService
{
    public class DoctorService : IDoctorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDoctorReposatory doctorReposatory;
        private readonly IFileService fileService;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IUnitofwork unitofwork;
        private readonly IDoctorAttachmentRepository doctorAttachmentRepository;
        private readonly IRoleService roleService;
        private readonly ITokenService tokenService;
        private readonly IParentReposatory parentReposatory;
        private readonly IMemoryCache cache;
        private readonly IChildService childService;
        private readonly IParentService parentService;
        private readonly IChildReposatory childrepo;
        private readonly IUserMedicalRecordRepository userMedicalRecordRepository;
        private readonly IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository;

        public DoctorService(UserManager<ApplicationUser> userManager,
                            IDoctorReposatory doctorReposatory,
                            IFileService fileService,
                            ApplicationDbContext applicationDbContext,
                            IUnitofwork unitofwork,
                            IDoctorAttachmentRepository doctorAttachmentRepository,
                            IRoleService roleService,
                            ITokenService tokenService,
                            IParentReposatory parentReposatory,
                            IMemoryCache cache,
                            IChildService childService,
                            IParentService parentService,
                            IChildReposatory childrepo,
                            IUserMedicalRecordRepository userMedicalRecordRepository,
                            IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository
                            )
        {
            this._userManager = userManager;
            this.doctorReposatory = doctorReposatory;
            this.fileService = fileService;
            this.applicationDbContext = applicationDbContext;
            this.unitofwork = unitofwork;
            this.doctorAttachmentRepository = doctorAttachmentRepository;
            this.roleService = roleService;
            this.tokenService = tokenService;
            this.parentReposatory = parentReposatory;
            this.cache = cache;
            this.childService = childService;
            this.parentService = parentService;
            this.childrepo = childrepo;
            this.userMedicalRecordRepository = userMedicalRecordRepository;
            this.userMedcalRecordImagesRepository = userMedcalRecordImagesRepository;
        }

        public async Task<ApiResponse<string>> DeleteDoctor(DeleteDoctor deleteDoctor, CancellationToken cancellationToken)
        {
            var CheckForDoctor =await doctorReposatory.GetByNationalIdAsync(deleteDoctor.NationalId, cancellationToken);
            if(CheckForDoctor == null||CheckForDoctor.IsDeleted==true||CheckForDoctor.IsApproved==false)
                return ApiResponse<string>.ErrorResponse("Doctor not found", 404);
            var DoctorAttachmentUrl = await doctorAttachmentRepository.GetByDoctorIdAsync(CheckForDoctor.Id, cancellationToken);
            if(DoctorAttachmentUrl != null)
            {
                var deleteFileResult = await fileService.DeleteFile(DoctorAttachmentUrl);
                if (!deleteFileResult)
                    return ApiResponse<string>.ErrorResponse("Failed to delete doctor attachment");
            }
            CheckForDoctor.IsDeleted = true;
            var DeleteDoctorResult = await doctorReposatory.Update(CheckForDoctor, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<string>.ErrorResponse("Failed to delete doctor");
            cache.Remove($"Doctor_{deleteDoctor.NationalId}");
            return ApiResponse<string>.Success("Doctor deleted successfully");
        }

        public async Task<ApiResponse<AuthResultDto>> DoctorRegister(CreateDoctor createDoctor, CancellationToken cancellationToken)
        {
            try
            {
                var errors = new List<string>();

                if (createDoctor.Passworded != createDoctor.ConfirmPassworded)
                    errors.Add("Passwords do not match");

                var existingUser = await _userManager.FindByEmailAsync(createDoctor.Email);
                if (existingUser != null)
                {
                    var UserRole = await roleService.GetUserRole(existingUser.Email);
                    foreach (var result in UserRole.Data)
                    {
                        if (result == "Doctor")
                            return ApiResponse<AuthResultDto>.ErrorResponse("Email already exists");
                    }
                }

                var existingDoctor = await doctorReposatory.GetByNationalIdAsync(createDoctor.NationalId, cancellationToken);
                if (existingDoctor != null)
                    errors.Add("National ID already exists");

                if (errors.Any())
                    return ApiResponse<AuthResultDto>.ErrorResponse(errors);

                var doctor = new Doctor
                {
                    FullName = createDoctor.FullName,
                    NationalId = createDoctor.NationalId,
                    BirthDate = createDoctor.BirthDate,
                    Village = createDoctor.Village,
                    City = createDoctor.City,
                    Gender = createDoctor.Gender,
                    Governorate = createDoctor.Governorate,
                    BloodType = createDoctor.BloodType,
                    PhoneNumber = createDoctor.PhoneNumber,
                    Email = createDoctor.Email,
                    CreateAt = DateTime.UtcNow,
                };

                var existingParent = await parentReposatory.GetByNationalIdAsync(createDoctor.NationalId, cancellationToken);


                using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    if (existingParent == null)
                    {
                        var parent = new Parent
                        {
                            FullName = createDoctor.FullName,
                            NationalId = createDoctor.NationalId,
                            BirthDate = createDoctor.BirthDate,
                            Village = createDoctor.Village,
                            City = createDoctor.City,
                            BloodType = createDoctor.BloodType,
                            CreateAt = DateTime.UtcNow,
                            Gender = createDoctor.Gender,
                            Governorate = createDoctor.Governorate,
                            PhoneNumber = createDoctor.PhoneNumber,
                            Email = createDoctor.Email,

                        };
                        await parentReposatory.AddAsync(parent, cancellationToken);
                        var resultOfParent = await unitofwork.SaveChangesAsync(cancellationToken);

                        if (resultOfParent <= 0)
                            return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new Parent");


                    }

                    await doctorReposatory.AddAsync(doctor, cancellationToken);
                    var result = await unitofwork.SaveChangesAsync(cancellationToken);

                    if (result <= 0)
                        return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new Doctor");

                    var doctorAttachmentUrl = await fileService.SaveFileAsync(createDoctor.DoctorAttachment);

                    var doctorAttachment = new DoctorAttachment
                    {
                        DoctorId = doctor.Id,
                        FilePath = doctorAttachmentUrl,
                    };

                    await doctorAttachmentRepository.AddAsync(doctorAttachment, cancellationToken);
                    var resultOfSavingDoctorAttachment = await unitofwork.SaveChangesAsync(cancellationToken);

                    if (resultOfSavingDoctorAttachment <= 0)
                        return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving doctor attachment");
                    var CheckForUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NationalId == createDoctor.NationalId, cancellationToken);
                    var token = string.Empty;

                    if (CheckForUser == null)
                    {
                        var user = new ApplicationUser
                        {
                            FullName = createDoctor.FullName,
                            UserName = createDoctor.NationalId,
                            Email = createDoctor.Email,
                            PhoneNumber = createDoctor.PhoneNumber,
                            NationalId = createDoctor.NationalId
                        };

                        var newUser = await _userManager.CreateAsync(user, createDoctor.Passworded);
                        if (!newUser.Succeeded)
                            return ApiResponse<AuthResultDto>.ErrorResponse(newUser.Errors.Select(e => e.Description).ToList());
                        await roleService.EnsureRoleExistsAsync("Doctor");
                        await roleService.AssignRoleAsync(user, "Doctor");
                        if (existingParent == null)
                        {
                            await roleService.EnsureRoleExistsAsync("Parent");
                            await roleService.AssignRoleAsync(user, "Parent");
                        }
                        token = await tokenService.GenerateToken(user, cancellationToken);

                    }
                    else
                    {
                        await roleService.EnsureRoleExistsAsync("Doctor");
                        await roleService.AssignRoleAsync(CheckForUser, "Doctor");
                        token = await tokenService.GenerateToken(CheckForUser, cancellationToken);

                    }

                    await transaction.CommitAsync(cancellationToken);

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
            catch (Exception ex)
            {
                return ApiResponse<AuthResultDto>.ErrorResponse(ex.Message);
            }
        }

       


        #region DoctorDashboard


        public async Task<ApiResponse<GetDoctorForDashboardResponse>> GetDoctorForDashboard(string DoctorAspNetUserId, CancellationToken cancellationToken)
        {
            var user =await _userManager.FindByIdAsync(DoctorAspNetUserId);
            if (user == null)
                return ApiResponse<GetDoctorForDashboardResponse>.ErrorResponse("Doctor not found", 404);
            var doctor = await doctorReposatory.GetByNationalIdAsync(user.NationalId, cancellationToken);
            if (doctor == null)
                return ApiResponse<GetDoctorForDashboardResponse>.ErrorResponse("Doctor not found", 404);
            if (doctor.IsApproved == false)
                return ApiResponse<GetDoctorForDashboardResponse>.ErrorResponse("Doctor Not Approved", 400);
            GetDoctorForDashboardResponse response = new GetDoctorForDashboardResponse
            {
                Id = doctor.Id,
                Name = doctor.FullName
            };
            return ApiResponse<GetDoctorForDashboardResponse>.Success(response);

        }
        public async Task<ApiResponse<UserResponseForDashboard>> GetUserDetailsAsync(string NationalId, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.NationalId == NationalId);
            if (user == null)
                return ApiResponse<UserResponseForDashboard>.ErrorResponse("User not found", 404);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("Parent"))
            {
                var parent = await parentReposatory.GetByNationalIdAsync(NationalId, cancellationToken);
                if (parent == null)
                    return ApiResponse<UserResponseForDashboard>.ErrorResponse("Parent not found", 404);
                var result = await parentService.GetParentDetailsAsync(parent.Id, cancellationToken);
                UserResponseForDashboard userResponseForDashboard = new UserResponseForDashboard
                {
                    Type = "Parent",
                    ParentDetails = result.Data
                };
                return ApiResponse<UserResponseForDashboard>.Success(userResponseForDashboard);
            }
            else if (userRoles.Contains("Child"))
            {
                var child = await childrepo.GetByNationalIdAsync(NationalId, cancellationToken);
                if (child == null)
                    return ApiResponse<UserResponseForDashboard>.ErrorResponse("Child not found", 404);
                var result = await childService.GetChildDetailsAsync(child.Id, cancellationToken);
                UserResponseForDashboard userResponseForDashboard = new UserResponseForDashboard
                {
                    Type = "Child",
                    ChildDetails = result.Data
                };
                return ApiResponse<UserResponseForDashboard>.Success(userResponseForDashboard);
            }
            return ApiResponse<UserResponseForDashboard>.ErrorResponse("User role not recognized", 400);
        }
        public async Task<ApiResponse<ChildVaccinationsResponse>> GetChildVaccinationsAsync(int ChildId, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildVaccinationsAsync(ChildId, cancellationToken);
            return result;
        }
        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetChildMedicalHistoryAsync(int ChildId, CancellationToken cancellationToken)
        {
            var result = await childService.GetChildMedicalHistoryAsync(ChildId, cancellationToken);
            return result;
        }
        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificChildMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await childService.GetSpecificChildMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return result;
        }
        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> AddChildMedicalRecord(AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken)
        {
            UserMedicalRecord userMedicalRecord = new UserMedicalRecord
            {
                ChildId = addUserMedicalRecord.ChildId,
                DoctorId = addUserMedicalRecord.DoctorId,
                Description = addUserMedicalRecord.Description,
                Medicine = addUserMedicalRecord.Medicine,
                CreatedAt = DateTime.UtcNow
            };



            using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var addChildMedicalRecordResult = await userMedicalRecordRepository.AddAsync(userMedicalRecord, cancellationToken);
                if (addChildMedicalRecordResult == null)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to add medical record", 400);
                var resultOfSavingMedicalRecord = await unitofwork.SaveChangesAsync(cancellationToken);
                if (resultOfSavingMedicalRecord <= 0)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to save medical record", 400);

                var AddingAttacment = await fileService.SaveFileAsync(addUserMedicalRecord.userMedicalImages);
                MedicalRecordImage userMedcalRecordImages = new MedicalRecordImage
                {
                    UserMedicalRecordId = addChildMedicalRecordResult.Id,
                    ImagePath = AddingAttacment,
                    CreatedAt = DateTime.UtcNow,
                };
                var addMedicalRecordImageResult = await userMedcalRecordImagesRepository.AddAsync(userMedcalRecordImages, cancellationToken);
                if (addMedicalRecordImageResult == null)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to add medical record image", 400);
                var resultOfSavingMedicalRecordImage = await unitofwork.SaveChangesAsync(cancellationToken);
                if (resultOfSavingMedicalRecordImage <= 0)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to save medical record image", 400);
                List<string>? Images = new List<string>();
                Images.Add(AddingAttacment);
                int userId = addUserMedicalRecord.ChildId.HasValue ? addUserMedicalRecord.ChildId.Value : 0;
                var doctor = await doctorReposatory.GetByIdAsync(addUserMedicalRecord.DoctorId, cancellationToken);
                if (doctor == null)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Doctor not found", 404);
                var response = new SpecificUserMedicalHistoryResponse
                {

                    UserId = userId,
                    MedicalRecordId = addChildMedicalRecordResult.Id,
                    Description = addChildMedicalRecordResult.Description,
                    Medicine = addChildMedicalRecordResult.Medicine,
                    GivenAt = addChildMedicalRecordResult.CreatedAt,
                    MedicalRecordImages = Images,
                    DoctorName=doctor.FullName,
                };
                await transaction.CommitAsync(cancellationToken);
                return ApiResponse<SpecificUserMedicalHistoryResponse>.Success(response, 201);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse(ex.Message);
            }




        }





        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetParentMedicalHistoryAsync(ParentId, cancellationToken);
            return result;
        }
        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            var result = await parentService.GetSpecificParentMedicalHistoryAsync(MedicalRecordId, cancellationToken);
            return result;
        }
        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> AddParentMedicalRecord(AddUserMedicalRecord addUserMedicalRecord, CancellationToken cancellationToken)
        {
            UserMedicalRecord userMedicalRecord = new UserMedicalRecord
            {
                ParentId = addUserMedicalRecord.ParentId,
                DoctorId = addUserMedicalRecord.DoctorId,
                Description = addUserMedicalRecord.Description,
                Medicine = addUserMedicalRecord.Medicine,
                CreatedAt = DateTime.UtcNow
            };



            using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var addParentMedicalRecordResult = await userMedicalRecordRepository.AddAsync(userMedicalRecord, cancellationToken);
                if (addParentMedicalRecordResult == null)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to add medical record", 400);
                var resultOfSavingMedicalRecord = await unitofwork.SaveChangesAsync(cancellationToken);
                if (resultOfSavingMedicalRecord <= 0)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to save medical record", 400);

                var AddingAttacment = await fileService.SaveFileAsync(addUserMedicalRecord.userMedicalImages);
                MedicalRecordImage userMedcalRecordImages = new MedicalRecordImage
                {
                    UserMedicalRecordId = addParentMedicalRecordResult.Id,
                    ImagePath = AddingAttacment,
                    CreatedAt = DateTime.UtcNow,
                };
                var addMedicalRecordImageResult = await userMedcalRecordImagesRepository.AddAsync(userMedcalRecordImages, cancellationToken);
                if (addMedicalRecordImageResult == null)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to add medical record image", 400);
                var resultOfSavingMedicalRecordImage = await unitofwork.SaveChangesAsync(cancellationToken);
                if (resultOfSavingMedicalRecordImage <= 0)
                    return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Failed to save medical record image", 400);
                List<string>? Images = new List<string>();
                Images.Add(AddingAttacment);
                int userId = addUserMedicalRecord.ParentId.HasValue ? addUserMedicalRecord.ParentId.Value : 0;
                var response = new SpecificUserMedicalHistoryResponse
                {

                    UserId = userId,
                    MedicalRecordId = addParentMedicalRecordResult.Id,
                    Description = addParentMedicalRecordResult.Description,
                    Medicine = addParentMedicalRecordResult.Medicine,
                    GivenAt = addParentMedicalRecordResult.CreatedAt,
                    
                    MedicalRecordImages = Images
                };
                await transaction.CommitAsync(cancellationToken);
                return ApiResponse<SpecificUserMedicalHistoryResponse>.Success(response, 201);

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse(ex.Message);
            }


        }





        public async Task<ApiResponse<GetDoctorResponse>> GetDoctor(int DoctorId, CancellationToken cancellationToken)
        {
            var result = await doctorReposatory.GetByIdAsync(DoctorId, cancellationToken);
            if (result == null)
                return ApiResponse<GetDoctorResponse>.ErrorResponse("Doctor not found", 404);
            if (result.IsDeleted)
                return ApiResponse<GetDoctorResponse>.ErrorResponse("Doctor not found", 404);
            if (!result.IsApproved)
                return ApiResponse<GetDoctorResponse>.ErrorResponse("Doctor not approved yet", 403);
            var response = new GetDoctorResponse
            {
                Id = result.Id,
                FullName = result.FullName,
                NationalId = result.NationalId,
                BirthDate = result.BirthDate,
                Village = result.Village.ToString(),
                City = result.City.ToString(),
                BloodType = result.BloodType.ToString(),
                CreateAt = result.CreateAt,
                Email = result.Email,
                Gender = result.Gender.ToString(),
                Governorate = result.Governorate.ToString(),
                PhoneNumber = result.PhoneNumber
            };
           
            return ApiResponse<GetDoctorResponse>.Success(response);
        }
        public async Task<ApiResponse<UpdateDoctorResponse>> UpdateDoctor(UpdateDoctor updateDoctor, CancellationToken cancellationToken)
        {

            var CheckForDoctor = await doctorReposatory.GetByNationalIdAsync(updateDoctor.NationalId, cancellationToken);
            if (CheckForDoctor == null)
                return ApiResponse<UpdateDoctorResponse>.ErrorResponse("Doctor not found", 404);
            CheckForDoctor.FullName = updateDoctor.FullName;
            //CheckForDoctor.NationalId = updateDoctor.NationalId;
            CheckForDoctor.BirthDate = updateDoctor.BirthDate;
            CheckForDoctor.Village = updateDoctor.Village;
            CheckForDoctor.City = updateDoctor.City;
            CheckForDoctor.Gender = updateDoctor.Gender;
            CheckForDoctor.Governorate = updateDoctor.Governorate;
            CheckForDoctor.BloodType = updateDoctor.BloodType;
            CheckForDoctor.PhoneNumber = updateDoctor.PhoneNumber;
            CheckForDoctor.Email = updateDoctor.Email;
            CheckForDoctor.CreateAt = DateTime.UtcNow;
            CheckForDoctor.IsApproved = false;

            var UpdateDoctorResult = await doctorReposatory.Update(CheckForDoctor, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<UpdateDoctorResponse>.ErrorResponse("Failed to update doctor");
            cache.Remove($"Doctor_{updateDoctor.NationalId}");

            var response = new UpdateDoctorResponse()
            {
                FullName = updateDoctor.FullName,
                NationalId = updateDoctor.NationalId,
                BirthDate = updateDoctor.BirthDate,
                Village = updateDoctor.Village,
                City = updateDoctor.City,
                Gender = updateDoctor.Gender.ToString(),
                Governorate = updateDoctor.Governorate.ToString(),
                BloodType = updateDoctor.BloodType.ToString(),
                PhoneNumber = updateDoctor.PhoneNumber,
                Email = CheckForDoctor.Email,
                CreateAt = DateTime.UtcNow,
                Id = CheckForDoctor.Id
            };
            return ApiResponse<UpdateDoctorResponse>.Success(response);

        }


        #endregion

    }
}