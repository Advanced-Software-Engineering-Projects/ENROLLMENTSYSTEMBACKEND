using ENROLLMENTSYSTEMBACKEND.DTOs;
using ENROLLMENTSYSTEMBACKEND.Services;
using Microsoft.AspNetCore.Mvc;

namespace ENROLLMENTSYSTEMBACKEND.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("generate")]
        public ActionResult<string> GenerateToken([FromBody] TokenRequestDto request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Role))
            {
                return BadRequest("UserId, Email, and Role are required.");
            }

            var token = _tokenService.GenerateToken(request.UserId, request.Email, request.Role);
            return Ok(token);
        }
    }
}
