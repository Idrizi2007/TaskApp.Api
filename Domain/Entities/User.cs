namespace TaskApp.Api.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public UserRole Role { get; private set; } = UserRole.User;

        public RefreshToken? RefreshToken { get; private set; }

        private User() { } // EF Core

        public User(string email, string passwordHash, UserRole role = UserRole.User)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public void SetRefreshToken(RefreshToken refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
