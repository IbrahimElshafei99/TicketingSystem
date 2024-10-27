using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Business.Interfaces;
using Ticketing.Core.DTO;

namespace TicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegistrationDTO userRegister)
        {
            if(ModelState.IsValid)
            {
                var result = await _authService.RegesterUser(userRegister);
                if (result)
                    return Ok("User registered successfully");
                return BadRequest("User already registered");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "Registeration Failed, please try again",
                Status = "Error"
            });

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            if(ModelState.IsValid)
            {
                var token = await _authService.Login(userLogin);
                if (token == null)
                    return Unauthorized();
                return Ok(new { Token = token });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "Login Failed, please try again",
                Status = "Error"
            });

        }
    }
}

