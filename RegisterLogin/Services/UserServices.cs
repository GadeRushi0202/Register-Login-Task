using Microsoft.EntityFrameworkCore;
using RegisterLogin.Models;
using RegisterLogin.Repositry;
using System.Net.Mail;
using System.Net;

namespace RegisterLogin.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepo repo;
        private readonly IConfiguration _config;


        public UserServices(IUserRepo repo, IConfiguration config)
        {
            this.repo = repo;
            _config = config;
        }
        public int AddUsers(Users users)
        {
            return repo.AddUsers(users);
        }

        public int DeleteUsers(int id)
        {
            return repo.DeleteUsers(id);
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return repo.GetAllUsers();
        }

        /*public Users GetUser(string UserName, string Password)
        {
            return repo.GetUser(UserName, Password);
        }*/
        public Users GetUsers(string UserName)
        {
            return repo.GetUsers(UserName);
        }
        public Users GetUsersById(int id)
        {
            return repo.GetUsersById(id);
        }

        public bool IsUserNameUnique(string userName)
        {
            return repo.IsUserNameUnique(userName); 
        }

        public Users Login(Users users)
        {
            return repo.Login(users);
        }

        public int UpdateUsers(Users users)
        {
            return repo.UpdateUsers(users);
        }
    public void SendPasswordResetEmail(string userName)
    {
        var user = repo.GetUsers(userName);
        if (user != null)
        {
            string token = Guid.NewGuid().ToString(); // Generate a unique token
            repo.SetPasswordResetToken(userName, token);

            // Compose the reset URL (adjust according to your environment)
            string resetUrl = $"http://localhost:5030/User/ResetPassword?token={token}";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("gaderushi7393@gmail.com"),
                Subject = "Password Reset Request",
                Body = $"Click the following link to reset your password: {resetUrl}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(user.Email);

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("gaderushi7393@gmail.com", "tbid nxpy axgx ophp");
                smtpClient.EnableSsl = true;

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"SMTP Exception: {ex.Message}");
                }
            }
        }
    }

    public Users GetUserByPasswordResetToken(string token)
        {
            return repo.GetUserByPasswordResetToken(token);
        }

        public int ResetPassword(string token, string newPassword)
        {
            var user = repo.GetUserByPasswordResetToken(token);
            if (user != null)
            {
                user.Password = newPassword;
                user.PasswordResetToken = null;
                return repo.UpdateUsers(user);
            }
            return 0;
        }
    }
}
