using Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskApp.Api.Domain.Entities;
using TaskApp.Api.Infrastructure.Persistence;

namespace TaskApp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var user = new User(request.Email, string.Empty);

            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
            user = new User(request.Email, hashedPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponse(GenerateToken(user), null);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email)
                ?? throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            var refreshToken = new RefreshToken(
                GenerateRefreshToken(),
                DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["Jwt:RefreshTokenDays"]!)
                )
            );

            user.SetRefreshToken(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponse(
                GenerateToken(user),
                refreshToken.Token
            );
        }

        public async Task<AuthResponse> RefreshAsync(RefreshRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken != null &&
                    u.RefreshToken.Token == request.RefreshToken);

            if (user == null ||
                user.RefreshToken == null ||
                user.RefreshToken.IsExpired ||
                user.RefreshToken.IsRevoked)
            {
                throw new Exception("Invalid refresh token");
            }

            user.RefreshToken.Revoke();

            var newRefreshToken = new RefreshToken(
                GenerateRefreshToken(),
                DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["Jwt:RefreshTokenDays"]!)
                )
            );

            user.SetRefreshToken(newRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponse(
                GenerateToken(user),
                newRefreshToken.Token
            );
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user?.RefreshToken == null)
                return;

            user.RefreshToken.Revoke();
            await _context.SaveChangesAsync();
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64)
            );
        }

        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["Jwt:AccessTokenMinutes"]!)
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
