public class RefreshToken
{
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    private RefreshToken() { }

    public RefreshToken(string token, DateTime expiresAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public void Revoke()
    {
        IsRevoked = true;
    }
}
