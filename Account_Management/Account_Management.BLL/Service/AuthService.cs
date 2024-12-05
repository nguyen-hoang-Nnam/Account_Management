using Account_Management.BLL.Service.IService;
using Account_Management.DAL.Repository.IRepository;
using Account_Management.Models.Helper;
using Account_Management.Models.Models.DTO;
using Account_Management.Models.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Account_Management.Models.Models.DTO.Auth;
using Account_Management.Models.Enum;
using BCrypt.Net;

namespace Account_Management.BLL.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;
        private JwtHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, IMapper mapper, JwtHelper jwtHelper, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _userRepository = userRepository;
        }

        // Login
        public async Task<ResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var response = new ResponseDTO();

            var user = await _authRepository.GetByUserName(loginDTO.Username);
            if (user == null)
            {
                response.Message = "Invalid credentials";
                return response;
            }

            var isPasswordValid = VerifyPassword(loginDTO.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                response.Message = "Invalid credentials";
                return response;
            }

            var token = _jwtHelper.GenerateJwtToken(user);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            var tokenExpiration = DateTime.Now.AddHours(1);
            var refreshTokenExpiration = DateTime.Now.AddDays(7);

            user.Token = token;
            user.TokenExpires = tokenExpiration;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpires = refreshTokenExpiration;
            await _authRepository.UpdateAsync(user);

            response.IsSucceed = true;
            response.Message = "Login successful!";
            response.Data = new { Token = token, RefreshToken = refreshToken };

            return response;
        }


        // Register
        public async Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var response = new ResponseDTO();

            var existingUser = await _authRepository.GetByUserName(registerDTO.Username);
            if (existingUser != null)
            {
                response.Message = "User already exists!";
                return response;
            }

            var account = _mapper.Map<Account>(registerDTO);

            account.AccountId = GenerateUniqueId();
            account.PasswordHash = HashPassword(registerDTO.Password);
            account.IsActive = AccountStatus.Active;
            account.Role = AccountRole.Customer;
            account.Token = string.Empty;
            account.RefreshToken = string.Empty;
            account.CreatedAt = DateTime.Now;

            await _authRepository.AddAsync(account);

            var user = new User
            {
                AccountId = account.AccountId,
                FullName = registerDTO.FullName,
                PhoneNumber = registerDTO.PhoneNumber,
            };

            // Add the customer to the database
            await _userRepository.AddAsync(user);

            response.IsSucceed = true;
            response.Message = "Registration successful!";
            response.Data = true;
            return response;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); // Explicit namespace
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword); // Explicit namespace
        }

        private string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
