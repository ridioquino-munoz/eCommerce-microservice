using Domain;
using Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Business.Identity
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(IConfiguration configuration, ITokenRepository tokenRepository)
        {
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        public async Task<TokenRefresh> Add(TokenRefresh tokenRefresh)
        {
            return await _tokenRepository.Add(tokenRefresh);
        }

        public string GenerateAccessToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var audiences =_configuration
                .GetSection("Jwt:Audience")
                .GetChildren()
                .Select(x => x.Value)
                .ToArray();

            foreach (var audience in audiences) {
                claims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Aud, audience));
            }

            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
            _configuration["Jwt:Secret"]));

            var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: creds
            );

            return new JwtSecurityTokenHandler()
            .WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
            new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out SecurityToken securityToken);

            return principal;
        }

        public async Task<TokenRefresh?> GetByUserID(Guid userID)
        {
            return  await _tokenRepository.GetByUserID(userID);
        }

        public Task<TokenRefresh> Update(TokenRefresh tokenRefresh)
        {
            return _tokenRepository.Update(tokenRefresh);
        }

        public async Task<bool> Remove(Guid userID)
        {
            return await _tokenRepository.Delete(userID);
        }
    }

    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task<TokenRefresh> Add(TokenRefresh tokenRefresh);
        Task<TokenRefresh> Update(TokenRefresh tokenRefresh);
        Task<TokenRefresh?> GetByUserID(Guid userID);

        Task<bool> Remove(Guid userID);

    }
}
