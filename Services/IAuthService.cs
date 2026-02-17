using Contracts.Auth;

namespace TaskApp.Api.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RefreshAsync(RefreshRequest request);
        Task LogoutAsync(Guid userId);
    }
}
