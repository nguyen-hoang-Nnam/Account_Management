using Account_Management.Models.Models.DTO;
using Account_Management.Models.Models.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.BLL.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<ResponseDTO> LoginAsync(LoginDTO loginDTO);
    }
}
