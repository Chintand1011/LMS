using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using OPBids.Web.Models;

namespace OPBids.Web.Logic.Auth.Manager
{
    public class CustomUserStore<T> : IUserStore<T> where T : AuthUser
    {      
        public Task CreateAsync(T user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}