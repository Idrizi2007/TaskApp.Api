using Contracts.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                throw new Exception("Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User(request.Email, passwordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponse> RefreshAsync(RefreshRequest request)
        {
            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.RefreshToken != null &&
                                          u.RefreshToken.Token == request.RefreshToken);

            if (user is null)
                throw new Exception("Invalid refresh token");

            if (user.RefreshToken!.IsExpired || user.RefreshToken.IsRevoked)
                throw new Exception("Refresh token expired or revoked");

            // Revoke old token
            user.RefreshToken.Revoke();

            return await GenerateTokensAsync(user);
        }

        public async Task LogoutAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null || user.RefreshToken is null)
                return;

            user.RefreshToken.Revoke();
            await _context.SaveChangesAsync();
        }

        private async Task<AuthResponse> GenerateTokensAsync(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            var refreshDays = int.Parse(_configuration["Jwt:RefreshTokenDays"]!);

            user.SetRefreshToken(new RefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(refreshDays)
            ));

            await _context.SaveChangesAsync();

            return new AuthResponse(accessToken, refreshToken);
        }

        private string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var minutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"]!);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}