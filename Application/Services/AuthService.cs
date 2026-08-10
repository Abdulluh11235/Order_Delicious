using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Services.Interfaces;
using Common;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace Application.Services;

public class AuthService:IAuthService
{
    private readonly UserManager<ApplicationUser> _userManger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
  
    public AuthService( UserManager<ApplicationUser> userManger,
        RoleManager<IdentityRole> roleManage,IConfiguration configuration)
    {
        _userManger = userManger; 
        _roleManager = roleManage;
        _configuration = configuration;
    }
    public async Task<Result<AuthModel>> Register(RegisterModel model)
    {
        if (await _userManger.FindByEmailAsync(model.Email) is not null)
            return new Result<AuthModel>(false) { ErrorMessage = "Email is already Registered" };
        if (await _userManger.FindByNameAsync(model.Username) is not null)
            return new Result<AuthModel>(false) { ErrorMessage = "Username is already Registered"  };
        
        var user = new ApplicationUser()
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        
        
        var refreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(refreshToken);
        var res=await _userManger.CreateAsync(user, model.Password);

        if (!res.Succeeded)
        {
            var errors = string.Join(',', res.Errors.Select(x => x.Description));
           return new Result<AuthModel>(false) { ErrorMessage = errors };
        }
        
        await _userManger.AddToRoleAsync(user,Roles.User);
        var token = await CreateJwtToken(user);
        
 
            var val= new AuthModel()
            {
                Email = user.Email,
                Roles = new List<string>() { Roles.User },
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn,
                Username = user.UserName
            };
        return new Result<AuthModel>() {Value = val};
    }

    public async Task<Result<AuthModel>> GetToken(TokenRequestModel model)
    {
        var user = await _userManger.FindByEmailAsync(model.Email);
        
        var invalidCredRes= new Result<AuthModel>(false) { ErrorMessage = "Email Or Password is incorrect." };
        
        if (user is null || 
        !await _userManger.CheckPasswordAsync(user, model.Password) ) 
            return invalidCredRes;
        var token = await CreateJwtToken(user);
        
        var authModel = new AuthModel()
        {
            Email = user.Email!,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Username = user.UserName!,
            Roles = await _userManger.GetRolesAsync(user)
        };
        
        if (user.RefreshTokens.Any(t=>t.IsActive))
        {
            var activeRefreshToken = user.RefreshTokens.First(t=>t.IsActive);
            authModel.RefreshToken = activeRefreshToken.Token;
            authModel.RefreshTokenExpiresOn = activeRefreshToken.ExpiresOn;
        }
        else
        {
          var refreshToken = GenerateRefreshToken();
          authModel.RefreshToken = refreshToken.Token;
          authModel.RefreshTokenExpiresOn = refreshToken.ExpiresOn;
          user.RefreshTokens.Add(refreshToken);
          
          await _userManger.UpdateAsync(user);
        }
       
        return new Result<AuthModel>() {Value = authModel};
    }

    public async Task<Result<string>> AddRole(AddRoleModel model)
    {
        var user = await _userManger.FindByIdAsync(model.UserId);
        if(user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
            return new Result<string>(false) 
            { ErrorMessage = "Invalid User Id Or Role" };
        if(await _userManger.IsInRoleAsync(user, model.RoleName))
            return new Result<string>(false) { ErrorMessage = "User Already In Role" };
       
        var val = await _userManger.AddToRoleAsync(user, model.RoleName);
       
      return (!val.Succeeded)?
             new Result<string>(false)
                { ErrorMessage = "Something Went Wrong" }
          : new Result<string>() {Value = ""};
    }



    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManger.GetClaimsAsync(user);
        var roles = await _userManger.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id)
        }.Union(userClaims).Union(roleClaims);

        var secret = GetJwtConfigValue("Key");
        var issuer = GetJwtConfigValue("Issuer");
        var audience = GetJwtConfigValue("Audience");
        var durationInMinutes = GetJwtConfigValue("DurationInMinutes");

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(durationInMinutes)),
            signingCredentials: signingCredentials
        );
        
        return jwtSecurityToken;
    }

    private string GetJwtConfigValue(string key)
    {
        var value = _configuration[$"JWT:{key}"];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing JWT configuration: 'JWT:{key}'. Add it in appsettings.json.");
        }
        return value;
    }

    private RefreshToken GenerateRefreshToken()
    {
     var randNumber = RandomNumberGenerator.GetBytes(32);
     return new RefreshToken()
     {
        Token =Convert.ToBase64String(randNumber),
        ExpiresOn = DateTime.UtcNow.AddDays(10),
        CreatedOn = DateTime.UtcNow,    
     };
    }
    public async Task<Result<AuthModel>> RefreshToken(string token)
    {
        var authModel = new AuthModel();
         var user = await _userManger.Users.SingleOrDefaultAsync(u=>u.RefreshTokens.Any(t => t.Token == token));
        if (user is null)
        {
            return new Result<AuthModel>(false){ ErrorMessage = "Invalid Token" };
        }
        var refreshToken = user.RefreshTokens.Single(t=>t.Token == token);
       
        if(!refreshToken.IsActive)
        {
            return new Result<AuthModel>(false){ ErrorMessage = "Inactive Token" };
        }
        refreshToken.RevokedOn = DateTime.UtcNow;
        
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManger.UpdateAsync(user);
        
        var jwtSecurityToken = await CreateJwtToken(user);
        
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.Email = user.Email;
        authModel.Username = user.UserName;
        authModel.Roles = (await _userManger.GetRolesAsync(user) ).ToList();
        authModel.RefreshToken = newRefreshToken.Token;
        authModel.RefreshTokenExpiresOn = newRefreshToken.ExpiresOn;

        return new Result<AuthModel>(true){ Value = authModel};
    }

    public async Task<Result<string>> RevokeToken(string token)
    {
        var authModel = new AuthModel();
        var user = await _userManger.Users.SingleOrDefaultAsync(u=>u.RefreshTokens.Any(t => t.Token == token));
        if (user is null)
            return new Result<string>(false){ ErrorMessage = "Invalid Token" };
        
        var refreshToken = user.RefreshTokens.Single(t=>t.Token == token);
       
        if(!refreshToken.IsActive)
            return new Result<string>(false){ ErrorMessage = "Inactive Token" };

        refreshToken.RevokedOn = DateTime.UtcNow;
        
        await _userManger.UpdateAsync(user);
        
        return new Result<string>(true){ Value = ""};
    }
}
