using HealthAxis.Api.Data;
using HealthAxis.Api.Helpers;
using HealthAxis.Api.Repositories.Interfaces;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HealthAxisEntities _context;
        public UserRepository(HealthAxisEntities context) { _context = context; }
        public User1 GetByEmail(string email) { return _context.User1.FirstOrDefault(u => u.Email == email); }
        public User1 Add(User1 user) { _context.User1.Add(user); _context.SaveChanges(); return user; }
        public bool UpdateReferenceId(string userId, int referenceId)
        {
            var user = _context.User1.Find(userId);
            if (user == null) return false;
            user.ReferenceId = referenceId;
            _context.SaveChanges();
            return true;
        }
        public string GenerateNextUserId(string role)
        {
            string prefix = role == "Patient" ? "P" : "D";
            string last = _context.User1.Where(u => u.UserId.StartsWith(prefix)).OrderByDescending(u => u.UserId).Select(u => u.UserId).FirstOrDefault();
            return UserIdGenerator.Next(prefix, last);
        }
    }
}
