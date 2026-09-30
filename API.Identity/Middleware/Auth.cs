using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Identity.Middleware
{
    public class Auth
    {
        private readonly ILogger<Auth> _logger;
        private readonly RequestDelegate _next;

        public Auth(RequestDelegate next, ILogger<Auth> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"]
            .FirstOrDefault();

            if (!string.IsNullOrEmpty(authHeader) &&
            authHeader.StartsWith("Bearer "))
            {
                var token = authHeader["Bearer ".Length..];

                try
                {
                    var handler = new JwtSecurityTokenHandler();

                    if (handler.ReadToken(token) is JwtSecurityToken jwtToken)
                    {
                        var userId = jwtToken.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
                        ?.Value;

                        _logger.LogInformation(
                        "Authenticated UserId: {UserId}",
                        userId);

                        context.Items["UserId"] = userId;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Invalid JWT Token");
                }
            }

            await _next(context);
        }

    }
}
