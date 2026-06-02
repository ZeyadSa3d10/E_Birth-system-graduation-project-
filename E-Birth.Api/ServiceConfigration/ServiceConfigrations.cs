using E_Birth.Application.Authservice;
using E_Birth.Application.ChildService;
using E_Birth.Application.Configratin;
using E_Birth.Application.DoctorService;
using E_Birth.Application.FileService;
using E_Birth.Application.HospitalService;
using E_Birth.Application.NotificationService;
using E_Birth.Application.ParentService;
using E_Birth.Application.RoleService;
using E_Birth.Application.TokenService;
using E_Birth.Domain.Interfaces.Reposatories;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.Identity;
using E_Birth.Infrastructure.DbContext;
using E_Birth.Infrastructure.Reposatories;
using E_Birth.Infrastructure.Repositories;
using E_Birth.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace E_Birth.Api.ServiceConfigration
{
    public class ServiceConfigrations
    {
        public static void ConfigureService(IConfiguration configuration, IServiceCollection services)
        {
            AddDatabase(services, configuration);
            AddDIForReposatories(services);
            AddAuthentication(services, configuration);
            AddVersioning(services);
            AddDIForServices(services);
            AddFrontend(services);
            AddOpenApiDocument(services);
        }

        private static IServiceCollection AddOpenApiDocument(IServiceCollection services)
        {
            //string[] versions = ["v1"];

            //foreach (var version in versions)
            //{
            //    services.AddOpenApi(version, options =>
            //    {
            //        // Versioning config
            //        options.AddDocumentTransformer<VersionInfoTransformer>();

            //        // Security Scheme config

            //        options.AddDocumentTransformer<BearerSecuritySchemeFormer>();
            //        options.AddOperationTransformer<BearerSecuritySchemeFormer>();
            //    });
            //}
            return services;
        }

        private static void AddDIForReposatories(IServiceCollection services)
        {
            services.AddScoped<IChildReposatory, ChildRepository>();
            services.AddScoped<IChildVaccinationRepository, ChildVaccinationRepository>();
            services.AddScoped<IAllOfficialVaccinationReposatory, AllOfficialVaccinationReposatory>();
            services.AddScoped<IDoctorReposatory, DoctorRepository>();
            services.AddScoped<IDoctorAttachmentRepository, DoctorAttachmentRepository>();
            services.AddScoped<IParentReposatory, ParentRepository>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IUserMedicalRecordRepository, UserMedicalRecordRepository>();
            services.AddScoped<IUserMedcalRecordImagesRepository, UserMedcalRecordImagesRepository>();
            services.AddScoped<IPasswordResetCodeGenerateReposatory, PasswordResetCodeGenerateReposatory>();
            services.AddScoped<IUnitofwork, UnitOfWork>();
        }
        private static void AddDIForServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IChildService, ChildService>();
        }
        public static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            //// In Program.cs / DI registration
            //services.AddDbContextFactory<ApplicationDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("Local")));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Server"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>()
                    ?? throw new InvalidOperationException("JWT settings are not configured properly.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,


                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddAuthorization();
        }
        private static void AddVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
        }
        private static void AddFrontend(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Open", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });

                //options.AddPolicy("Open", policy =>
                //{
                //    policy
                //        .WithOrigins(
                //        "http://localhost:5173",
                //        "https://localhost:5173"
                //        )
                //        .AllowAnyHeader()
                //        .AllowAnyMethod();
                //});
            });
        }
    }
}
