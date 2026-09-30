using API.Identity.DTO;
using Business.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace API.Identity.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserRequest request)
        {
            var passwordHasher = new PasswordHasher<User>();

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                EmailAddress = request.EmailAddress,
                ContactNumber = request.ContactNumber,
                UserName = request.UserName
            };

            user.Password = passwordHasher.HashPassword(user, request.Password);
            user.Role = request.Role;
            
            var userAdded = await _userService.Add(user);

            return Ok(userAdded);
        }

        [HttpPut]
        [Route("udpateprofile/{id:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid id,UpdateUserRequest request)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Address = request.Address;
            user.EmailAddress = request.EmailAddress;
            user.ContactNumber = request.ContactNumber;

            var updatedUser = await _userService.Update(user);

            return Ok(updatedUser);
        }

        [HttpPut("updatepassword/{id:guid}")]
        public async Task<IActionResult> UpdatePassword(Guid id,UpdateUserPasswordRequest request)
        {
            var user = await _userService.GetById(id);
            
            if (user == null) { 
                return NotFound();
            }

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user,request.Password);
            var updatedUser = await _userService.Update(user);

            return Ok(updatedUser);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _userService.Remove(id));
        }
    }
}
