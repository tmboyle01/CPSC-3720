using TigerTix.Data.Entities;

namespace TigerTix.Data
{
    public interface IUserRepository
    {
        void DeleteUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserById(string cuid);
        void SaveUser(User user);
        void UpdateUser(User user,string CUID);
        bool SaveAll();

    }

}