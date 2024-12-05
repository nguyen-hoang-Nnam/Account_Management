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
using Google.Apis.Auth;

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
        public async Task<ResponseDTO> LoginWithGoogleAsync(string googleToken)
        {
            var response = new ResponseDTO();

            var googleAuthHelper = new GoogleAuthHelper();
            var payload = await googleAuthHelper.ValidateGoogleTokenAsync(googleToken);

            if (payload == null)
            {
                response.IsSucceed = false;
                response.Message = "Invalid Google token.";
                return response;
            }

            var account = await _authRepository.GetByEmail(payload.Email);

            if (account == null)
            {
                // Register a new account if it doesn't exist
                var newAccount = new Account
                {
                    AccountId = GenerateUniqueId(),
                    Email = payload.Email,
                    Username = payload.Email, // Use email as the username
                    IsActive = AccountStatus.Active,
                    Role = AccountRole.Customer,
                    Token = string.Empty,
                    RefreshToken = string.Empty,
                    CreatedAt = DateTime.UtcNow,
                };

                await _authRepository.AddAsync(newAccount);

                // Add a corresponding User entry
                var user = new User
                {
                    AccountId = newAccount.AccountId,
                    FullName = payload.Name,
                    PhoneNumber = null // Optional: populate later
                };

                await _userRepository.AddAsync(user);

                account = newAccount;
            }

            // Generate JWT and refresh tokens
            var token = _jwtHelper.GenerateJwtToken(account);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            account.Token = token;
            account.TokenExpires = DateTime.UtcNow.AddHours(1);
            account.RefreshToken = refreshToken;
            account.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);

            await _authRepository.UpdateAsync(account);

            response.IsSucceed = true;
            response.Message = "Google login successful!";
            response.Data = new { Token = token, RefreshToken = refreshToken };

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
