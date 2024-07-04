using Application.Helper;
using Product.Application.Dtos;

namespace Product.Application.Services.Contracts
{
    public interface IAuthService
    {
        Task<SuccessResponse<LoginResponse>> Login(LoginDto model);
        Task<SuccessResponse<RegisterResponse>> RegisterUser(RegisterDto model);
    }
}
