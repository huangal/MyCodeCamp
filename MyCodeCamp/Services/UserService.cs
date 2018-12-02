using System.Collections.Generic;
using System.Linq;
using MyCodeCamp.Models;

namespace MyCodeCamp.Services
{
    public class UserService : IUserService
    {
        public IEnumerable<User> GetUsers()
        {
            // return users without passwords
            return _users.Select(x => { x.Password = null; return x; });
        }
     
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Henry", LastName = "Huangal", Username = "test", Password = "test" }
        };


        public User GetUser(string username, string password )
        {
           return  _users.SingleOrDefault(x => x.Username == username && x.Password == password);
        }

    }
}
