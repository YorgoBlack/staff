using Staff.Data;
using Staff.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Staff.Services
{
    public class UsersService : IUserService
    {
        readonly AppDbContext _context;
        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AppUser> GetUsers()
        {
            return _context.AppUsers;
        }

        public void Create(AppUser user)
        {
            user.LastActionTime = DateTime.Now;
            _context.Add(user);
            _context.SaveChanges();
        }
        public void Delete(AppUser user)
        {
            _context.Remove(user);
            _context.SaveChanges();
        }
        public void Update(AppUser user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public AppUser Authenticate(string username, string password)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.UserName == username && x.Password == password);
            if (user == null)
                return null;
            return new AppUser() { Id = user.Id, UserName = user.UserName };
        }
    }
}
