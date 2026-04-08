using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services.Interfaces;
using Common;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
            ExpiresOn = token.ValidTo,
            Roles = new List<string>() { Roles.User },
            Token = new JwtSecurityTokenHandler().WriteToken(token),
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
            ExpiresOn = token.ValidTo,
            Roles = await _userManger.GetRolesAsync(user)
        };
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
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("uid", user.Id)
        }.Union(userClaims).Union(roleClaims);
     
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:DurationInDays"])),
            signingCredentials: signingCredentials
        );
        
        return jwtSecurityToken;
    }

}
