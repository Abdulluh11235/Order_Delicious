using Domain;

namespace Application.Services.Interfaces;

public interface IAuthService
{
    Task<Result<AuthModel>> Register(RegisterModel model);
    Task<Result<AuthModel>> GetToken(TokenRequestModel model);
    Task<Result<string>> AddRole(AddRoleModel model);
}
