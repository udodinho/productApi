using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Application.Helper;
using Application.Helpers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Product.Application.Dtos;
using Product.Application.Services.Contracts;
using Product.Domain.Entities;
using Product.Infrastructure.Repository;

namespace Product.Application.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;


        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<SuccessResponse<LoginResponse>> Login(LoginDto model)
        {

            var loginData = await _unitOfWork.Users.FirstOrDefaultNoTracking(p => p.Email == model.Email);

            if (loginData is null)
                throw new RestException(HttpStatusCode.BadRequest, $"User not found ", "Email");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, loginData.Password))
                throw new RestException(HttpStatusCode.BadRequest, $"Wrong Password", "Password");


            var token = await CreateToken(loginData);

            var res = new LoginResponse { Token = token , UserId = loginData.Id};
            return new SuccessResponse<LoginResponse>()
            {
                Data = res,
                Message = "Login Successful"
            };
        }

        public async Task<SuccessResponse<RegisterResponse>> RegisterUser(RegisterDto model)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var check_user = _unitOfWork.Users.Exists(p => p.Email == model.Email);


            if (check_user == true)
                throw new RestException(HttpStatusCode.BadRequest, $"Email Address already exist for a user", "Email");


            var user = _mapper.Map<User>(model);
            user.Id = Guid.NewGuid().ToString();
            user.Password = passwordHash;
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var res = new RegisterResponse
            {
                Token = await CreateToken(user),
                UserId = user.Id
            };


            return new SuccessResponse<RegisterResponse>()
            {
                Data = res,
                Message = "User Registration was Successful"
            };
        }

        private async Task<string> CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
            {

                new Claim("UserId", user.Id?? ""),
                new Claim("FirstName", user.FirstName?? ""),
                new Claim("Email", user.Email ?? ""),
                new Claim("LastName", user.LastName ?? ""),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                   signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
