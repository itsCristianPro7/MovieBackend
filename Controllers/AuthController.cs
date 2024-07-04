using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDataAccess.Models;
using MovieBackend.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieBackend.Controllers
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

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errors);
            }


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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errors);
            }


            var result = await _authService.Login(user);
            if (result)
            {
                var token = await _authService.GenerateTokenAsync(user);
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(UserRole userRole)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errors);
            }

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
