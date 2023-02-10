using AutoMapper;
using BlogAPI.Biz.Models;
using BlogAPI.Biz.Services.Authentication;
using BlogAPI.Contracts.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authenticationService,
            IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        /// <param name="request">Request object with user log in information</param>
        /// <returns>Authentication result</returns>
        /// <response code="200">User logged in</response>
        /// <response code="500">Server failed</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResult))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var authResponse = await _authenticationService
                .LoginAsync(request.Email, request.Password);
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Registers a user in the application
        /// </summary>
        /// <param name="request">New user object</param>
        /// <returns>Authentication result</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            try
            {
                var userPerson = _mapper.Map<UserPerson>(request);
                var authResponse = await _authenticationService.RegisterAsync(userPerson);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}
