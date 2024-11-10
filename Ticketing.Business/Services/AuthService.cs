using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Business.Interfaces;
using Ticketing.Core.DTO;
using Ticketing.Core.Interfaces;
using Ticketing.Data.Models;

namespace Ticketing.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly JwtOptions _jwtOptions;

        public AuthService(IUserRepo userRepo,IOptions<JwtOptions> jwtOptions)
        {
            _userRepo = userRepo;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<bool> RegesterUser(UserRegistrationDTO user)
        {
            var isSuccessed = await _userRepo.AddUser(user);
            return isSuccessed;
        }

        public async Task AddUserRole()
        {

        }

        public async Task<string> Login(UserLoginDTO loginDTO)
        {
            var user = await _userRepo.FindUserBy(loginDTO.PhoneNumber);
            if (user == null)
            {
                return null;
            }
            return await GenerateJWT(user);
        }

        private async Task<string> GenerateJWT(User user)
        {
            var userRole = await _userRepo.GetRoleById(user.RoleId);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, userRole),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var token = new JwtSecurityToken( 
                issuer: _jwtOptions.Issure,
                audience: _jwtOptions.Audience,
                claims: authClaims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                expires: DateTime.UtcNow.AddHours(3)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }


    }
}
