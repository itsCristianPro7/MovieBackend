using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using Movies.Api.Domain;
using Movies.Api.Interfaces;
using Movies.Api.Models;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> Register(User user)
        {
            var appUser = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(appUser, user.Password);

            return result.Succeeded;
        }

        public async Task<(bool succeeded, ApplicationUser appUser, string token)> Login(User user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(appUser, user.Password);

            if (appUser is null || !passwordValid)
            {
                return (false, null, null);
            }

            string tokenString = await GenerateTokenAsync(appUser);
            
            return (true, appUser, tokenString);
        }

        public async Task<bool> AssignRole(UserRoleAssignment userRoleAssignment)
        {
            var appUser = await _userManager.FindByEmailAsync(userRoleAssignment.Email);
            if (appUser is null)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(appUser, userRoleAssignment.RoleName);
            return result.Succeeded;
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser appUser)
        {
            var roles = await _userManager.GetRolesAsync(appUser);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(CustomClaimTypes.Uid, appUser.Id.ToString())
            }
            .Union(roleClaims);

            var jwtKey = _config["JwtSettings:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSettings:Duration"])),
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                signingCredentials:signingCredentials
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }
    }
}
