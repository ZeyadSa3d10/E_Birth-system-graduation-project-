using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Birth.Application.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager,
                           UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ApiResponse<string>> EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (!result.Succeeded)
                    return ApiResponse<string>.ErrorResponse("Faild To Add Role");
            }
                return ApiResponse<string>.Success($"Role : { roleName} Added Success");
        }

        public async Task<ApiResponse<string>> AssignRoleAsync(ApplicationUser user, string roleName)
        {
           var resultOfCheck= await EnsureRoleExistsAsync(roleName);
            if (resultOfCheck.IsSuccess==false)
            {
                return ApiResponse<string>.ErrorResponse("Faild To Add User To Role");
            }
           var result = await _userManager.AddToRoleAsync(user, roleName);
            if(!result.Succeeded)
                return ApiResponse<string>.ErrorResponse("Faild To Add User To Role");
            return ApiResponse<string>.Success(roleName);
        }

        public async Task<ApiResponse<string>> RemoveRoleFromUserAsync(ApplicationUser user, string roleName)
        {
           var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                return ApiResponse<string>.ErrorResponse($"Faild To Remove Role : {roleName} From User");
            return ApiResponse<string>.Success($"Role : {roleName} Remove From {user.Email}");
        }

        public async Task<ApiResponse<string>> UpdateRole(string OldRole, string NewRole)
        {
           var role = _roleManager.FindByNameAsync(OldRole).Result;
            if (role == null)
                return ApiResponse<string>.ErrorResponse($"Role {OldRole} Not Found");
            role.Name = NewRole;
            var result = _roleManager.UpdateAsync(role).Result;
            if (!result.Succeeded)
                return ApiResponse<string>.ErrorResponse($"Role {OldRole} Not Found");
            return ApiResponse<string>.Success($"Old Role : {OldRole} New Role : {NewRole}");
        }

        public async Task<ApiResponse<List<string>>> GetAllRolesAsync()
        {
            var results = _roleManager.Roles.Select(r => r.Name).ToList();
            if (results.Count == 0)
                return ApiResponse<List<string>>.ErrorResponse("No Roles Found");
            return ApiResponse<List<string>>.Success(results);
        }

        public async Task<ApiResponse<string>> DeleteRoleAsync(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName).Result;
            if (role == null)
                return ApiResponse<string>.ErrorResponse($"Role {roleName} Not Found");
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                return ApiResponse<string>.ErrorResponse($"Faild To Remove Role : {roleName}");
            return ApiResponse<string>.Success($"Role : {roleName} Deleted");
        }

        public async Task<ApiResponse<List<string>>> GetUserRole(string email)
        {
            var user =await _userManager.FindByEmailAsync(email);
            if (user == null)
                return ApiResponse<List<string>>.ErrorResponse("User Not Found" ,404);
            var roles =await _userManager.GetRolesAsync(user);
            return ApiResponse<List<string>>.Success(roles.ToList());
        }
    }

}
