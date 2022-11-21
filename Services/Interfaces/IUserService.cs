using Services.Contracts;
using Services.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse> ChangePassword(ChangePasswordDto input);
        Task<ServiceResponse<UserDto>> CreateAsync(CreateUpdateUserDto input);
        Task<ServiceResponse> DeleteAsync(Guid id);
        Task<ServiceResponse<UserDto>> GetAsync();
        Task<ServiceResponse<UserLoginResponseDto>> LoginAsync(UserLoginRequestDto request);
        Task<ServiceResponse> UpdateAsync(Guid id, CreateUpdateUserDto input);
    }
}
