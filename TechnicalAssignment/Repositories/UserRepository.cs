using Microsoft.EntityFrameworkCore;
using TechnicalAssignment.DBContext;
using TechnicalAssignment.Models;

namespace TechnicalAssignment.Repositories
{
    public class UserRepository
    {
        private readonly InMemoryDbContext _context;

        public UserRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

    }
}
