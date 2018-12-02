using System.Collections.Generic;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public interface IUserService
    {
       
        IEnumerable<User> GetUsers();
        User GetUser(string username, string password);

    }
}
