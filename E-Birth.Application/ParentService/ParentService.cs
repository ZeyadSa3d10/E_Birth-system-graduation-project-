using E_Birth.Application.RoleService;
using E_Birth.Application.TokenService;
using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.ChildDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Enums;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Repositories;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Birth.Application.ParentService
{
    public class ParentService : IParentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IParentReposatory parentReposatory;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IUnitofwork unitofwork;
        private readonly IRoleService roleService;
        private readonly ITokenService tokenService;
        private readonly IChildReposatory childReposatory;
        private readonly IDoctorReposatory doctorReposatory;
        private readonly IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository;
        private readonly IUserMedicalRecordRepository userMedicalRecordRepository;

        public ParentService(UserManager<ApplicationUser> userManager ,
                             IParentReposatory parentReposatory,
                             ApplicationDbContext applicationDbContext,
                             IUnitofwork unitofwork,
                             IRoleService roleService,
                             ITokenService tokenService,
                             IChildReposatory childReposatory,
                             IDoctorReposatory doctorReposatory,
                             IUserMedcalRecordImagesRepository userMedcalRecordImagesRepository,
                             IUserMedicalRecordRepository userMedicalRecordRepository
                             )
        {
            this._userManager = userManager;
            this.parentReposatory = parentReposatory;
            this.applicationDbContext = applicationDbContext;
            this.unitofwork = unitofwork;
            this.roleService = roleService;
            this.tokenService = tokenService;
            this.childReposatory = childReposatory;
            this.doctorReposatory = doctorReposatory;
            this.userMedcalRecordImagesRepository = userMedcalRecordImagesRepository;
            this.userMedicalRecordRepository = userMedicalRecordRepository;
        }
        //public async Task<ApiResponse<AuthResultDto>> ParentRegister(CreateParent createParent, CancellationToken cancellationToken)
        //{
        //    var emailTask = _userManager.FindByEmailAsync(createParent.Email);
        //    var nationalIdTask = parentReposatory.GetByNationalIdAsync(createParent.NationalId, cancellationToken);

        //    await Task.WhenAll(emailTask, nationalIdTask);

        //    var existingUser = emailTask.Result;   // ApplicationUser?
        //    var existingParent = nationalIdTask.Result; // Parent?

        //    var errors = new List<string>();
        //    if (createParent.Passworded != createParent.ConfirmPassworded)
        //        errors.Add("Passwords do not match");
        //    if (existingUser != null)
        //        errors.Add("Email already exists");
        //    if (existingParent != null)
        //        errors.Add("National ID already exists");
        //    if (errors.Any())
        //        return ApiResponse<AuthResultDto>.ErrorResponse(errors);

        //    // 2. Build both entities before opening the transaction
        //    var parent = new Parent
        //    {
        //        FullName = createParent.FullName,
        //        NationalId = createParent.NationalId,
        //        BirthDate = createParent.BirthDate,
        //        Village = createParent.Village,
        //        City = createParent.City,
        //        Gender = createParent.Gender,
        //        Governorate = createParent.Governorate,
        //        BloodType = createParent.BloodType,
        //        PhoneNumber = createParent.PhoneNumber,
        //        Email = createParent.Email,
        //        CreateAt = DateTime.UtcNow
        //    };

        //    var user = new ApplicationUser
        //    {
        //        FullName = createParent.FullName,
        //        UserName = createParent.NationalId,
        //        Email = createParent.Email,
        //        PhoneNumber = createParent.PhoneNumber,
        //        NationalId = createParent.NationalId
        //    };

        //    // 3. Ensure the role exists BEFORE the transaction — it's a one-time idempotent op
        //    await roleService.EnsureRoleExistsAsync("Parent");

        //    using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
        //    try
        //    {
        //        // 4. Add parent to the change tracker (no DB hit yet)
        //        await parentReposatory.AddAsync(parent, cancellationToken);

        //        // 5. CreateAsync internally saves the user — batch the parent save with it
        //        //    by flushing SaveChanges only once after both are staged
        //        var newUser = await _userManager.CreateAsync(user, createParent.Passworded);
        //        if (!newUser.Succeeded)
        //        {
        //            await transaction.RollbackAsync(cancellationToken);
        //            return ApiResponse<AuthResultDto>.ErrorResponse(
        //                newUser.Errors.Select(e => e.Description).ToList());
        //        }

        //        // 6. Single SaveChanges call flushes the parent insert
        //        var result = await unitofwork.SaveChangesAsync(cancellationToken);
        //        if (result <= 0)
        //        {
        //            await transaction.RollbackAsync(cancellationToken);
        //            return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new parent");
        //        }

        //        // 7. AssignRole is now the only remaining DB write inside the transaction
        //        await roleService.AssignRoleAsync(user, "Parent");

        //        await transaction.CommitAsync(cancellationToken);

        //        var token = await tokenService.GenerateToken(user, cancellationToken);
        //        return ApiResponse<AuthResultDto>.Success(new AuthResultDto { Token = token }, 201);
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync(cancellationToken);
        //        return ApiResponse<AuthResultDto>.ErrorResponse(ex.Message);
        //    }
        //}

        public async Task<ApiResponse<AuthResultDto>> ParentRegister(CreateParent createParent, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            if (createParent.Passworded != createParent.ConfirmPassworded)
                errors.Add("Passwords do not match");

            var existingUser = await _userManager.FindByEmailAsync(createParent.Email);
            if (existingUser != null)
                errors.Add("Email already exists");

            var existingParent = await parentReposatory.GetByNationalIdAsync(createParent.NationalId, cancellationToken);
            if (existingParent != null)
                errors.Add("National ID already exists");

            if (errors.Any())
                return ApiResponse<AuthResultDto>.ErrorResponse(errors);

            var parent = new Parent
            {
                FullName = createParent.FullName,
                NationalId = createParent.NationalId,
                BirthDate = createParent.BirthDate,
                Village = createParent.Village,
                City = createParent.City,
                Gender = createParent.Gender,
                Governorate = createParent.Governorate,
                BloodType = createParent.BloodType,
                PhoneNumber = createParent.PhoneNumber,
                Email = createParent.Email,
                CreateAt = DateTime.UtcNow
            };

            using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await parentReposatory.AddAsync(parent, cancellationToken);
                var result = await unitofwork.SaveChangesAsync(cancellationToken);

                if (result <= 0)
                    return ApiResponse<AuthResultDto>.ErrorResponse("Error occurred while saving new parent");

                var user = new ApplicationUser
                {
                    FullName = createParent.FullName,
                    UserName = createParent.NationalId,
                    Email = createParent.Email,
                    PhoneNumber = createParent.PhoneNumber,
                    NationalId = createParent.NationalId
                };

                var newUser = await _userManager.CreateAsync(user, createParent.Passworded);
                if (!newUser.Succeeded)
                    return ApiResponse<AuthResultDto>.ErrorResponse("Error Ocure While Saving");
                await roleService.EnsureRoleExistsAsync("Parent");
                await roleService.AssignRoleAsync(user, "Parent");

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
        public async Task<ApiResponse<IEnumerable<ParentWithChildResponse>>> GetParentWithChilderen(string ParentAspNetUserId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(ParentAspNetUserId);
            if (user == null)
                return ApiResponse<IEnumerable<ParentWithChildResponse>>.ErrorResponse("User not found", 404);
            var parent = await parentReposatory.GetByNationalIdAsync(user.NationalId, cancellationToken);
            if (parent == null)
                return ApiResponse<IEnumerable<ParentWithChildResponse>>.ErrorResponse("Parent not found", 404);
            var Children = await childReposatory.GetByParentNationalIdAsync(parent.NationalId, cancellationToken);
            var response = new List<ParentWithChildResponse>();
            ParentWithChildResponse parentWithChildResponse = new ParentWithChildResponse
            {
                Id = parent.Id,
                FullName = parent.FullName,
                BirthDate = parent.BirthDate,
                AgeWithMonths = CalculateAge(parent.BirthDate).Months,
                AgeWithYears = CalculateAge(parent.BirthDate).Years,
                Gender = parent.Gender.ToString(),
                role= "Parent"
            };
            response.Add(parentWithChildResponse);
            foreach (var child in Children)
            {
                ParentWithChildResponse NewChild = new ParentWithChildResponse
                {
                    Id = child.Id,
                    FullName = child.FullName,
                    BirthDate = child.BirthDate,
                    AgeWithMonths = CalculateAge(child.BirthDate).Months,
                    AgeWithYears = CalculateAge(child.BirthDate).Years,
                    Gender = child.Gender.ToString(),
                    role = "Child"
                };
                response.Add(NewChild);
            }
            return ApiResponse<IEnumerable<ParentWithChildResponse>>.Success(response);
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

        public async Task<ApiResponse<ParentDetailsResponse>> GetParentDetailsAsync(int ParentId, CancellationToken cancellationToken)
        {
            var parent = await parentReposatory.GetByIdAsync(ParentId, cancellationToken);
            if (parent == null)
                return ApiResponse<ParentDetailsResponse>.ErrorResponse("Parent Not Found", 404);
            ParentDetailsResponse parentDetails = new ParentDetailsResponse()
            {
                ParentId = parent.Id,
                FullName = parent.FullName,
                NationalId = parent.NationalId,
                BirthDate = parent.BirthDate,
                Gender = parent.Gender.ToString(),
                BloodType = parent.BloodType.ToString(),
                Governorate = parent.Governorate.ToString(),
                Village = parent.Village,
                City = parent.City,
                PhoneNumber=parent.PhoneNumber,
                Email= parent.Email
            };
            return ApiResponse<ParentDetailsResponse>.Success(parentDetails);
        }

        public async Task<ApiResponse<UserMedicalHistoryResponse>> GetParentMedicalHistoryAsync(int ParentId, CancellationToken cancellationToken)
        {
            var parent = await parentReposatory.GetByIdAsync(ParentId, cancellationToken);
            if (parent == null)
                return ApiResponse<UserMedicalHistoryResponse>.ErrorResponse("Parent Not Found", 404);
            var age = CalculateAge(parent.BirthDate);
            var userMedicalRecords = await userMedicalRecordRepository.GetAllAsync(m => m.ParentId == ParentId, cancellationToken);

            var userDetails = new List<UserDetails>();


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
                UserId = parent.Id,
                UserName=parent.FullName,
                UserAgeInMonths = age.Months,
                UserAgeInYears = age.Years,
                Gender = parent.Gender.ToString(),
                MedicalHistory = userDetails
            };
            return ApiResponse<UserMedicalHistoryResponse>.Success(response, 200);
        }

        public async Task<ApiResponse<SpecificUserMedicalHistoryResponse>> GetSpecificParentMedicalHistoryAsync(int MedicalRecordId, CancellationToken cancellationToken)
        {
            var MedicalRecord = await userMedicalRecordRepository.GetByIdAsync(MedicalRecordId, cancellationToken);
            if (MedicalRecord == null)
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Medical Record Not Found", 404);
            var doctor = await doctorReposatory.GetByIdAsync(MedicalRecord.DoctorId, cancellationToken);
            if (doctor == null)
                return ApiResponse<SpecificUserMedicalHistoryResponse>.ErrorResponse("Doctor Not Found", 404);

            int ParentId = int.Parse(MedicalRecord.ParentId.ToString());
            var userMedcalRecordImages = await userMedcalRecordImagesRepository.GetAllAsync(i => i.UserMedicalRecordId == MedicalRecordId, cancellationToken);

            var response = new SpecificUserMedicalHistoryResponse
            {
                UserId = ParentId,
                MedicalRecordId = MedicalRecord.Id,
                DoctorName = doctor.FullName,
                GivenAt = MedicalRecord.CreatedAt,
                Description = MedicalRecord.Description,
                Medicine = MedicalRecord.Medicine,
                MedicalRecordImages = userMedcalRecordImages.Select(i => i.ImagePath).ToList()
            };
            return ApiResponse<SpecificUserMedicalHistoryResponse>.Success(response, 200);
        }
        public async Task<ApiResponse<ParentDetailsResponse>> UpdateParentProfile(UpdateParent updateParent, CancellationToken cancellationToken)
        {
            var parent =await parentReposatory.GetByIdAsync(updateParent.Id, cancellationToken);
            if(parent == null)
                return ApiResponse<ParentDetailsResponse>.ErrorResponse("Parent Not Found", 404);
            if (parent.NationalId != updateParent.NationalId)
                return ApiResponse<ParentDetailsResponse>.ErrorResponse("Parent NationalId Not Correct");
            parent.BirthDate = updateParent.BirthDate;
            parent.Village= updateParent.Village;
            parent.City= updateParent.City;
            parent.Governorate= updateParent.Governorate;
            parent.Email=updateParent.Email;
            parent.PhoneNumber=updateParent.PhoneNumber;
            parent.BloodType=updateParent.BloodType;
            parent.CreateAt = DateTime.UtcNow;
            parent.FullName = updateParent.FullName;
            parentReposatory.Update(parent,cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<ParentDetailsResponse>.ErrorResponse("Error occurred while updating parent");
            ParentDetailsResponse parentDetails = new ParentDetailsResponse()
            {
                ParentId = parent.Id,
                FullName = parent.FullName,
                NationalId = parent.NationalId,
                BirthDate = parent.BirthDate,
                Gender = parent.Gender.ToString(),
                BloodType = parent.BloodType.ToString(),
                Governorate = parent.Governorate.ToString(),
                Village = parent.Village,
                City = parent.City,
                Email= parent.Email,
                PhoneNumber = parent.PhoneNumber,
            };
            return ApiResponse<ParentDetailsResponse>.Success(parentDetails);
        }

        public async Task<ApiResponse<string>> DeleteParent(DeleteParentRequest deleteParentRequest, CancellationToken cancellationToken)
        {
            var parent =await parentReposatory.GetByIdAsync(deleteParentRequest.Id, cancellationToken);
            if (parent == null)
                return ApiResponse<string>.ErrorResponse("Parent Not Found", 404);
           parent.IsDeleted = true;
           await parentReposatory.Update(parent, cancellationToken);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<string>.ErrorResponse("Error occurred while deleting parent");
            return ApiResponse<string>.Success("Parent deleted successfully");
        }

        public async Task<ApiResponse<string>> AddParent(AddParentFromHospitalDto addParentFromHospitalDto, CancellationToken cancellationToken)
        {
            var CheckParent = await parentReposatory.GetByNationalIdAsync(addParentFromHospitalDto.NationalId, cancellationToken);
            if (CheckParent != null)
                return ApiResponse<string>.ErrorResponse("Parent With NationalId Already Exist", 400);
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NationalId == addParentFromHospitalDto.NationalId);
            if (user != null)
                return ApiResponse<string>.ErrorResponse("A user with the same National ID already exists", 400);
            var NewUser = new ApplicationUser()
            {
                NationalId = addParentFromHospitalDto.NationalId,
                FullName = addParentFromHospitalDto.FullName,
                UserName = addParentFromHospitalDto.NationalId,
                Email = addParentFromHospitalDto.Email
            };
            var createUserResult = await _userManager.CreateAsync(NewUser, "EBirthRemote1#");
            if (!createUserResult.Succeeded)
                return ApiResponse<string>.ErrorResponse("Error occurred while creating user account for the Parent", 500);
            await roleService.AssignRoleAsync(NewUser, "Parent");
            var parent = new Parent()
            {
                FullName = addParentFromHospitalDto.FullName,
                NationalId = addParentFromHospitalDto.NationalId,
                BirthDate = addParentFromHospitalDto.BirthDate,
                Gender = addParentFromHospitalDto.Gender,
                BloodType = addParentFromHospitalDto.BloodType,
                Governorate = addParentFromHospitalDto.Governorate,
                City = addParentFromHospitalDto.City,
                Village = addParentFromHospitalDto.Village,
                CreateAt= DateTime.UtcNow,
                Email= addParentFromHospitalDto.Email,
                PhoneNumber= addParentFromHospitalDto.PhoneNumber,
                
                
            };
            var createdParent = await parentReposatory.AddAsync(parent, cancellationToken);
            if (createdParent == null)
                return ApiResponse<string>.ErrorResponse("Error occurred while creating Parent", 500);
            var result = await unitofwork.SaveChangesAsync(cancellationToken);
            if (result <= 0)
                return ApiResponse<string>.ErrorResponse("Error occurred while creating Parent", 500);
            return ApiResponse<string>.Success("Parent Created Succesful", 201);
        }

    }
}
