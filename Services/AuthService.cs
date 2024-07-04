using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDataAccess.Models;
using MovieBackend.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieBackend.Services
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

        public async Task<bool> Login(User user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(appUser, user.Password);
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

        public async Task<string> GenerateTokenAsync(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var appUser = await _userManager.FindByEmailAsync(user.Email);
            var roles = await _userManager.GetRolesAsync(appUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }

            var jwtKey = _config.GetSection("JwtSettings:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config.GetSection("JwtSettings:Issuer").Value,
                audience: _config.GetSection("JwtSettings:Audience").Value,
                signingCredentials:signingCredentials
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString;
        }
    }
}
