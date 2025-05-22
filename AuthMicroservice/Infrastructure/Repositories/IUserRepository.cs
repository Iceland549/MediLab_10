using System.Threading.Tasks;
using AuthMicroservice.Domain.Models;

namespace AuthMicroservice.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
