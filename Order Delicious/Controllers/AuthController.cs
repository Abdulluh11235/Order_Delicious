using Application;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Order_Delicious.Controllers;

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var res = await _authService.Register(model);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);

            return Ok(res.Value);
        }
        
        [HttpPost("role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRole(AddRoleModel model)
        {
            var res = await _authService.AddRole(model);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);
            
            return Ok(model);
        }
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetToken(TokenRequestModel model)
        {
            var res = await _authService.GetToken(model);
            if (!res.IsSuccess) return BadRequest(res.ErrorMessage);
            return Ok(res.Value);
        }
        
        
    }

