using E_Birth.Domain.Interfaces.Services;
using E_Birth.Domain.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Birth.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class RolesController :ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(IRoleService roleService,
                               UserManager<ApplicationUser> userManager)
        {
            _roleService = roleService;
            _userManager = userManager;
        }
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            var result = await _roleService.EnsureRoleExistsAsync(roleName);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var result = await _roleService.DeleteRoleAsync(roleName);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(string oldRole, string newRole)
        {
            var result = await _roleService.UpdateRole(oldRole, newRole);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound($"User with email {email} not found");

            var result = await _roleService.AssignRoleAsync(user, roleName);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("RemoveRoleFromUser")]
        public async Task<IActionResult> RemoveRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound($"User with email {email} not found");

            var result = await _roleService.RemoveRoleFromUserAsync(user, roleName);
            return StatusCode(result.StatusCode, result);
        }

    }
}
