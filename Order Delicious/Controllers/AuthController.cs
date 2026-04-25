using System.Net;
using Application;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Order_Delicious.Controllers
{
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
            
            SetRefreshTokenCookie(res.Value.RefreshToken,res.Value.RefreshTokenExpiresOn);
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
            
            if(!string.IsNullOrEmpty(res.Value.RefreshToken))
                SetRefreshTokenCookie(res.Value.RefreshToken,
                    res.Value.RefreshTokenExpiresOn);
            
            return Ok(res.Value);
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                return BadRequest("Doesn't contain refreshToken"); 
            var res = await _authService.RefreshToken(refreshToken);
            if(!res.IsSuccess) return BadRequest(res.ErrorMessage);
             SetRefreshTokenCookie(res.Value.RefreshToken,res.Value.RefreshTokenExpiresOn);
            return Ok(res.Value);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token)) return BadRequest("refreshToken Not Provided");
            var res=await  _authService.RevokeToken(token);
           
            if(!res.IsSuccess) 
                return BadRequest(res.ErrorMessage);
            
            return Ok();
        }
        
        private void SetRefreshTokenCookie(string refreshToken,DateTime refreshTokenExpiryTime)
        {
            var opts = new CookieOptions
            {
                HttpOnly =  true,
                Expires = refreshTokenExpiryTime.ToLocalTime(),
            };
            Response.Cookies.Append("refreshToken", refreshToken, opts);
        }
        
    }
    
}

