using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WakeyWakeyAPI.Models;

public interface IUserController : IGenericController<User>
{
    Task<ActionResult<LoginValidationResult>> ValidateLogin(UserLoginRequest loginRequest);
    Task<ActionResult<User>> Register(UserRegisterRequest registerRequest);
}