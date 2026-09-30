using API.Identity.DTO;
using Azure.Core;
using Business.Identity;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.RefreshToken);

            var userId = principal.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetById(Guid.Parse(userId));

            if (user == null)
                return Unauthorized();

            var refreshToken = await _tokenService.GetByUserID(user.ID);

            if(refreshToken == null) return Unauthorized();

            if (refreshToken.Token != request.RefreshToken)
                return Unauthorized();

            if (refreshToken.ExpiryDate <= DateTime.UtcNow)
                return Unauthorized();

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            refreshToken.Token = newRefreshToken;
            await _tokenService.Update(refreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userService.GetByEmail(request.Email);
            if (user == null) return Unauthorized();

            var passwordHasher = new PasswordHasher<User>();

            var verified = passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                request.Password
            );

            if (verified != PasswordVerificationResult.Success) return Unauthorized();

            var newaAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshToken = await _tokenService.GetByUserID(user.ID);

            if (refreshToken == null)
            {

                var token = new TokenRefresh
                {
                    UserID = user.ID,
                    Token = newRefreshToken,
                    ExpiryDate = DateTime.UtcNow.AddDays(7)
                };

                await _tokenService.Add(token);
            }
            else
            {
                refreshToken.Token = newRefreshToken;
                await _tokenService.Update(refreshToken);
            }

            return Ok(new
            {
                AccessToken = newaAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

            var removed = await _tokenService.Remove(Guid.Parse(userId));

            return Ok(removed);
        }
    }
}
