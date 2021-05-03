using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Staff.Data.Models;

namespace Staff.Data
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetUsers();
        void Create(AppUser user);
        void Delete(AppUser user);
        void Update(AppUser user);
        AppUser Authenticate(string username, string password);
    }
}
