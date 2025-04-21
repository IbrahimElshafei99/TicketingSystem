using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Business.Interfaces;
using Ticketing.Business.Services;
using Ticketing.Core.DTO;

namespace TicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IAuthService authService, IUserService userService, ILogger<UserController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
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

        [HttpGet]
        [Route("GetAssignedOperators")]
        public async Task<IActionResult> GetAssignedOperators(string city, DateTime date)
        {
            try
            {
                var operators = await _userService.GetAssignedOperators(city, date);
                if (operators != null)
                {
                    return Ok(operators);
                }
                throw new NullReferenceException("No operators found");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in getting assigned operators: {message}", ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}

