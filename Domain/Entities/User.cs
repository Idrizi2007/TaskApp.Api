namespace TaskApp.Api.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        // Refresh token (owned entity)
        public RefreshToken? RefreshToken { get; private set; }

        private User() { } // EF Core

        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
        }

        public void SetRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
