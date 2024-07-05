using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Domain;
using Movies.Api.Filters;
using Movies.Api.Interfaces;
using Movies.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Movies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private RoleManager<ApplicationRole> _roleManager;

        public AuthController(IAuthService authService, RoleManager<ApplicationRole> roleManager)
        {
            _authService = authService;
            _roleManager = roleManager;
        }

        [ValidateModel]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var result = await _authService.Register(user);
            if (result)
            {
                return Ok("User registered");
            }
            else
            {
                return BadRequest();
            }

        }

        [ValidateModel]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {
            var result = await _authService.Login(user);
            if (result.succeeded)
            {
                var response = new AuthResponse
                {
                    UserId = result.appUser.Id,
                    Email = result.appUser.Email,
                    Token = result.token
                };
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [ValidateModel]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(UserRole userRole)
        {
            var appRole = new ApplicationRole
            {
                Name = userRole.RoleName
            };

            var result = await _roleManager.CreateAsync(appRole);
            if (result.Succeeded)
            {
                return Ok("Role created");
            }
            else
            {
                var errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return BadRequest(errors);
            }
        }

        [ValidateModel]
        [Authorize]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(UserRoleAssignment userRoleAssignment)
        {
            var result = await _authService.AssignRole(userRoleAssignment);
            if (result)
            {
                return Ok("Role assigned");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
