using E_Birth.Domain.CommonResponses;
using E_Birth.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ApiResponse<string>> EnsureRoleExistsAsync(string roleName);
        Task<ApiResponse<List<string>>> GetUserRole(string email);
        Task<ApiResponse<string>> UpdateRole(string OldRole,string NewRole);
        Task<ApiResponse<List<string>>> GetAllRolesAsync();
        Task<ApiResponse<string>> DeleteRoleAsync(string roleName);
        Task<ApiResponse<string>> AssignRoleAsync(ApplicationUser user, string roleName);
        Task<ApiResponse<string>> RemoveRoleFromUserAsync(ApplicationUser user, string roleName);
    }
}
