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
        // Send a password reset email
        /* public void SendPasswordResetEmail(string userName)
         {
             var user = repo.GetUsers(userName);
             if (user != null)
             {
                 string token = Guid.NewGuid().ToString(); // Generate a unique token
                 repo.SetPasswordResetToken(userName, token);

                 // Compose the reset URL (adjust according to your environment)
                 string resetUrl = $"https://yourdomain.com/User/ResetPassword?token={token}";

                 // Send the email
                 var mailMessage = new MailMessage("your-email@example.com", user.Email);
                 mailMessage.Subject = "Password Reset Request";
                 mailMessage.Body = $"Click the following link to reset your password: {resetUrl}";

                 using (var smtpClient = new SmtpClient("smtp.your-email-provider.com"))
                 {
                     smtpClient.Credentials = new NetworkCredential("your-email@example.com", "your-email-password");
                     smtpClient.EnableSsl = true;
                     smtpClient.Send(mailMessage);
                 }
             }
         }

         // Get user by password reset token
         public Users GetUserByPasswordResetToken(string token)
         {
             return repo.GetUserByPasswordResetToken(token);
         }

         // Reset the password
         public int ResetPassword(string token, string newPassword)
         {
             var user = repo.GetUserByPasswordResetToken(token);
             if (user != null)
             {
                 user.Password = newPassword; // Set the new password
                 user.PasswordResetToken = null; // Clear the token
                 return repo.UpdateUsers(user); // Save changes
             }
             return 0; // User not found or token expired
         }
        */
        // Send a password reset email
       /* public void SendPasswordResetEmail(string userName)
        {
            var user = repo.GetUsers(userName);
            if (user != null)
            {
                string token = Guid.NewGuid().ToString();
                repo.SetPasswordResetToken(userName, token);

                string resetUrl = $"https://yourdomain.com/User/ResetPassword?token={token}";

                var mailMessage = new MailMessage(_config["Smtp:FromEmail"], user.Email)
                {
                    Subject = "Password Reset Request",
                    Body = $"Click the following link to reset your password: {resetUrl}",
                    IsBodyHtml = true
                };

                using (var smtpClient = new SmtpClient(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"])))
                {
                    smtpClient.Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]);
                    smtpClient.EnableSsl = bool.Parse(_config["Smtp:EnableSsl"]);
                    smtpClient.Send(mailMessage);
                }
            }
        }*/
    public void SendPasswordResetEmail(string userName)
    {
        var user = repo.GetUsers(userName);
        if (user != null)
        {
            string token = Guid.NewGuid().ToString(); // Generate a unique token
            repo.SetPasswordResetToken(userName, token);

            // Compose the reset URL (adjust according to your environment)
            string resetUrl = $"https://yourdomain.com/User/ResetPassword?token={token}";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your_email@gmail.com"),
                Subject = "Password Reset Request",
                Body = $"Click the following link to reset your password: {resetUrl}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(user.Email);

            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.Credentials = new NetworkCredential("your_email@gmail.com", "your_app_password");
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
