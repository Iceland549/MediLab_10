using System.Threading.Tasks;
using AuthMicroservice.Application.DTOs;

namespace AuthMicroservice.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(UserDto userDto);

    }
}
