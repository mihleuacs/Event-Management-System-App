using WebAPI.Models;

namespace WebAPI.Services.Interface
{
    public interface IAuthService
    {
        Task<(int, string)> Register(SignupModel signup, string role);
        Task<(int, string, string)> Login(LoginModel model);
    }
}
