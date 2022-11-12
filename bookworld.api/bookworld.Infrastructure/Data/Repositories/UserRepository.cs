using bookworld.core.Domain.Entities;
using bookworld.core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace bookworld.Infrastructure.Data.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        public UserRepository()
        {
        }

        public Task<bool> CheckPassword(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByName(string userName)
        {
            throw new NotImplementedException();
        }        
    }
}
