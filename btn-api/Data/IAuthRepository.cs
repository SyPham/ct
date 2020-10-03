using System.Threading.Tasks;
using btn_api.Models;

namespace btn_api.Data
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);
    }
}