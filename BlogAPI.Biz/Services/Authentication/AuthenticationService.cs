using System.Security.Cryptography;
using BlogAPI.Biz.Models;
using BlogAPI.Controllers;
using BlogAPI.Data;
using BlogAPI.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Biz.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly BlogAPIDbContext _dbContext;
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(BlogAPIDbContext dbContext,
            JWTSettings jwtSettings,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenValidationParameters tokenValidationParameters)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var loggingUser = await _userManager.FindByEmailAsync(email);
            if (loggingUser == null)
                return new AuthenticationResult { Errors = new[] { "A user with this email does not exist" } };

            var userHasValidPassword = await _signInManager.CheckPasswordSignInAsync(loggingUser, password, false);

            if (!userHasValidPassword.Succeeded)
                return new AuthenticationResult { Errors = new[] { "The username or password combination is wrong" } };

            return await GenerateAuthenticationResultForUserAsync(loggingUser);
        }


        public async Task<AuthenticationResult> RegisterAsync(UserPerson userPerson)
        {
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userPerson.Email);
            if (existingUser != null)
                return new AuthenticationResult
                {
                    Errors = new[] { "A user with this email address already exists" },
                    Success = false
                };

            var applicationUser = new ApplicationUser
            {
                UserName = userPerson.Email,
                Email = userPerson.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createdUser = await _userManager.CreateAsync(applicationUser, userPerson.Password);

            if (!createdUser.Succeeded)
                return new AuthenticationResult
                {
                    Errors = new[] { "Error creating the user" },
                    Success = false
                };

            var person = new Person
            {
                FirstName = userPerson.FirstName,
                LastName = userPerson.LastName,
                ApplicationUser = applicationUser,
                PersonTypeId = userPerson.PersonType
            };

            _dbContext.Add(person);

            var personType = _dbContext.PersonType.Where(x => x.PersonTypeId == person.PersonTypeId).FirstOrDefault();

            await _userManager.AddClaimAsync(applicationUser, new Claim("personType", personType.Description));
            var result = await _dbContext.SaveChangesAsync();

            if (result < 0)
            {
                await _userManager.DeleteAsync(applicationUser);
                return new AuthenticationResult
                {
                    Errors = new[] { "Error creating the user" },
                    Success = false
                };
            }

            return await GenerateAuthenticationResultForUserAsync(applicationUser);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddMonths(6)
            };

            await _dbContext.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

    }
}
