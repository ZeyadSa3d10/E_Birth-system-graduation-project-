using E_Birth.Application.Authservice;
using E_Birth.Application.Configratin;
using E_Birth.Application.NotificationService;
using E_Birth.Application.TokenService;
using E_Birth.Domain.ApplicationDtos.AuthDtos;
using E_Birth.Domain.ApplicationDtos.DoctorDtos;
using E_Birth.Domain.ApplicationDtos.HospitalDtos;
using E_Birth.Domain.ApplicationDtos.ParentDtos;
using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.ApplicationModels;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Birth.Application.Authservice
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IParentService parentService;
        private readonly IDoctorService doctorService;
        private readonly IHospitalService hospitalService;
        private readonly IPasswordResetCodeGenerateReposatory passwordResetCodeGenerateReposatory;
        private readonly IUnitofwork unitofwork;
        private readonly IEmailService emailService;
        private readonly IParentReposatory parentReposatory;

        public AuthService(UserManager<ApplicationUser> userManager,
                           IConfiguration configuration,
                           ITokenService tokenService,
                           ApplicationDbContext applicationDbContext,
                           IParentService parentService,
                           IDoctorService doctorService,
                           IHospitalService  hospitalService,
                           IPasswordResetCodeGenerateReposatory passwordResetCodeGenerateReposatory,
                           IUnitofwork unitofwork,
                           IEmailService emailService
                           )
        {
            _userManager = userManager;
            _configuration = configuration;
            _tokenService = tokenService;
            this.applicationDbContext = applicationDbContext;
            this.parentService = parentService;
            this.doctorService = doctorService;
            this.hospitalService = hospitalService;
            this.passwordResetCodeGenerateReposatory = passwordResetCodeGenerateReposatory;
            this.unitofwork = unitofwork;
            this.emailService = emailService;
        }

        public async Task<ApiResponse<AuthResultDto>> UserLogin(UserLogin userLogin, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.NationalId == userLogin.EmailOrNationalId, cancellationToken);
            
            if (user is null)
            {
                user = await _userManager.FindByEmailAsync(userLogin.EmailOrNationalId);
                if(user is null)
                {
                    return ApiResponse<AuthResultDto>
                  .ErrorResponse("Invalid Login Data", 401);
                }
            }
               

            var isPasswordValid = await _userManager
                .CheckPasswordAsync(user, userLogin.Password);

            if (!isPasswordValid)
                return ApiResponse<AuthResultDto>
                    .ErrorResponse("Invalid Login Data", 401);

            if (await _userManager.IsLockedOutAsync(user))
                return ApiResponse<AuthResultDto>
                    .ErrorResponse("Account is locked", 403);


            var token = await _tokenService.GenerateToken(user, cancellationToken);

           
            var result = new AuthResultDto
            {
                Token = token,
            };

            return ApiResponse<AuthResultDto>.Success(result, 200);
        }
        public async Task<ApiResponse<AuthResultDto>> ParentRegister(CreateParent createParent, CancellationToken cancellationToken)
        {
            return await parentService.ParentRegister(createParent, cancellationToken);
        }
        public async Task<ApiResponse<AuthResultDto>> DoctorRegister(CreateDoctor createDoctor, CancellationToken cancellationToken)
        {
            return await doctorService.DoctorRegister(createDoctor, cancellationToken);
        }
        public async Task<ApiResponse<AuthResultDto>> HospitalRegister(CreateHospital createHospital, CancellationToken cancellationToken)
        {
            return await hospitalService.HospitalRegister(createHospital, cancellationToken);
        }
        public async Task<ApiResponse<string>> ForgetPassword(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {

                await Task.Delay(1000, cancellationToken); 
                return ApiResponse<string>.ErrorResponse("Please Create Account.");

            }
            var Otp = new Random().Next(100000, 999999).ToString();

            var HashedOtp = BCrypt.Net.BCrypt.HashPassword(Otp);


            var OtpRecord = new OtpCode
            {
                HashCode = HashedOtp,
                CreateAt = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false,
                UserId = user.Id
            };

            await passwordResetCodeGenerateReposatory.AddAsync(OtpRecord, cancellationToken);
            var result =await unitofwork.SaveChangesAsync(cancellationToken);
            if(result <=0) return ApiResponse<string>.ErrorResponse("Error Occure While Saving In Otp Table");
            string body = $@"
                <h3>Password Reset</h3>
                <p>Use the Otp below to reset your password:</p>
                <p><b>Email:</b> {email}</p>
                <P><b>Otp : </b> {Otp}</p>

                <p>Go to the reset password page and enter this Otp To Reset Password.</p>
            ";

            await emailService.SendAsync(email, "Reset Your Password", body);

            return ApiResponse<string>.Success("Reset Otp sent to your email.");
        }
        public async Task<ApiResponse<string>> IsvalidOtp(SendOtp sendOtp, CancellationToken cancellationToken)
        {
            var record = await passwordResetCodeGenerateReposatory
              .GetLastCodeByEmail(sendOtp.Email, cancellationToken);

            if (record == null)
                return ApiResponse<string>.ErrorResponse("There Are No Otp");

            if (record.IsUsed)
                return ApiResponse<string>.ErrorResponse("Otp Is Used");


            if (record.ExpirationTime < DateTime.UtcNow)
                return ApiResponse<string>.ErrorResponse("Otp Is Used");


            bool isValid = BCrypt.Net.BCrypt.Verify(sendOtp.Otp, record.HashCode);

            if (!isValid)
                return ApiResponse<string>.ErrorResponse("Otp Is Not Valid");

            record.IsUsed = true;
            var result = await unitofwork.SaveChangesAsync(cancellationToken);

            return ApiResponse<string>.Success("Otp Is Correct");
        }
        public async Task<ApiResponse<string>> ResendOtp(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {

                await Task.Delay(1000, cancellationToken);
                return ApiResponse<string>.Success("Reset Otp sent to your email.");
            }

            var Otp = new Random().Next(100000, 999999).ToString();

            var HashedOtp = BCrypt.Net.BCrypt.HashPassword(Otp);


            var record = new OtpCode
            {
                HashCode = HashedOtp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10),
                CreateAt = DateTime.UtcNow,
                IsUsed = false,
                UserId = user.Id
            };

            var lastRecord = await passwordResetCodeGenerateReposatory.GetLastCodeByEmail(email, cancellationToken);
            lastRecord.IsUsed = true;

            await passwordResetCodeGenerateReposatory.AddAsync(record, cancellationToken);
            await unitofwork.SaveChangesAsync(cancellationToken);

            string body = $@"
        <h3>Password Reset</h3>
        <p>Use the Otp below to reset your password:</p>
        <p><b>Email:</b> {email}</p>
        <P><b>Otp : </b> {Otp}</p>

        <p>Go to the reset password page and enter this Otp To Reset Password.</p>
    ";

            await emailService.SendAsync(email, "Reset Your Password", body);

            return ApiResponse<string>.Success("Reset Otp sent to your email.");
        }
        public async Task<ApiResponse<string>> ResetPasswordAsync(ApplyNewPassword request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
                return ApiResponse<string>.ErrorResponse("Passwords do not match.");

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return ApiResponse<string>.ErrorResponse("User not found.");

            var removeResult = await _userManager.RemovePasswordAsync(user);

            if (!removeResult.Succeeded)
                return ApiResponse<string>.ErrorResponse("Failed to reset password.");

            var addResult = await _userManager.AddPasswordAsync(user, request.Password);

            if (!addResult.Succeeded)
                return ApiResponse<string>.ErrorResponse(
                    string.Join(",", addResult.Errors.Select(e => e.Description)));

            return ApiResponse<string>.Success("Password reset successfully.");
        }
    }
}
