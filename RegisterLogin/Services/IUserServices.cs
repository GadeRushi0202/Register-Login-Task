using RegisterLogin.Models;

namespace RegisterLogin.Services
{
    public interface IUserServices
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUsersById(int id);
        int AddUsers(Users users);
        int UpdateUsers(Users users);
        int DeleteUsers(int id);

        Users Login(Users users);

        //Users GetUser(string UserName, string Password);
        Users GetUsers(string UserName);

        bool IsUserNameUnique(string userName);


        void SendPasswordResetEmail(string userName);
        Users GetUserByPasswordResetToken(string token);
        int ResetPassword(string token, string newPassword);
    }
}
