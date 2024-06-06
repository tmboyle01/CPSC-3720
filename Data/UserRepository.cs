using Microsoft.EntityFrameworkCore;
using TigerTix.Data.Entities;


namespace TigerTix.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly TigerTixContext _context;



        public UserRepository(TigerTixContext context)
        {
            _context = context;
        }

        // Save user to DB
        public void SaveUser(User user)
        {
            byte[] test = { Convert.ToByte(0) };

            user.isAdmin = false;
            user.userImage = test;

            _context.Add(user);
            _context.SaveChanges();

        }
        // Returns all users from DB
        public IEnumerable<User> GetAllUsers()
        {
            var userlist = from user in _context.Users
                           select user;

            return userlist.ToList();
        }
        // Return a single user based out of unique ID
        public User GetUserById(string cuid)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == cuid);
        }

        // Update User
        public void UpdateUser(User userInfo, string CUID)
        {
            var user = _context.Users.FirstOrDefault(p => p.UserName == CUID);

            if (userInfo.FirstName != null)
            {
                user.FirstName = userInfo.FirstName;
            }

            if (userInfo.LastName != null)
            {
                user.LastName = userInfo.LastName;
            }

            if (userInfo.userImage != null)
            {
                user.userImage = userInfo.userImage;
            }
            
            _context.SaveChanges();

        }




        // Delete User
        public void DeleteUser(User user)
        {
            _context.Remove(user);
            _context.SaveChanges();
        }

        public bool SaveAll()
        {

            return _context.SaveChanges() > 0;
        }

    }
}

