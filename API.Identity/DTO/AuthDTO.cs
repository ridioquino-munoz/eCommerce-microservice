namespace API.Identity.DTO
{
    public class AuthDTO
    {
    }

    public record LoginRequest(string Email, string Password);
    public record LogoutRequest(string Email);

    public record RefreshTokenRequest(string AccessToken, string RefreshToken);
}
