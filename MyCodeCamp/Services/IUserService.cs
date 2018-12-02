using System.Collections.Generic;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public interface IUserService
    {
       
        IEnumerable<User> GetUsers();

    }
}
