using RegisterLogin.Models;

namespace RegisterLogin.Repositry
{
    public interface IUserRepo
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUsersById(int id);
        int AddUsers(Users users);
        int UpdateUsers(Users users);
        int DeleteUsers(int id);

        Users Login(Users users);

        //Users GetUser(string UserName, string Password);
        Users GetUsers(string UserName);
        //Users GetUsers(string UserName);

        bool IsUserNameUnique(string userName);

        // New method for forgot password
        void SetPasswordResetToken(string userName, string token);
        Users GetUserByPasswordResetToken(string token);
    }
}
