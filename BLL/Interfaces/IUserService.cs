using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterDto dto);
        Task<UserResponseDto> LoginAsync(UserLoginDto dto);
        Task<UserResponseDto> GetCurrentUserAsync(Guid userId);
        Task<UserResponseDto> UpdateProfileAsync(Guid userId, UserUpdateDto dto);

    }
}
