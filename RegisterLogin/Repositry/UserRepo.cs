using RegisterLogin.Data;
using RegisterLogin.Models;

namespace RegisterLogin.Repositry
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext db;
        public UserRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public int AddUsers(Users users)
        {
            db.user.Add(users); 
            int res = db.SaveChanges(); 
            return res;
        }

        public int DeleteUsers(int id)
        {
            int res = 0;
            var result = db.user.Where(u => u.Id == id).FirstOrDefault();
            if(result != null)
            {
                db.user.Remove(result);
                res = db.SaveChanges();
            }
            return res;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return db.user.ToList();
        }

        public Users GetUsers(string UserName)
        {
            return db.user.Where(u => u.UserName == UserName).SingleOrDefault();
        }
        public Users GetUsersById(int id)
        {
            var result = db.user.Where(u => u.Id == id).FirstOrDefault();
            return result;
        }

        public bool IsUserNameUnique(string userName)
        {
            return !db.user.Any(u => u.UserName == userName);
        }

        public Users Login(Users users)
        {
            var result = db.user
                         .Where(u => u.UserName == users.UserName && u.Password == users.Password )
                         .FirstOrDefault();
            return result;
        }

        public int UpdateUsers(Users users)
        {
            int res = 0;
            var result = db.user.Where(u => u.Id == users.Id).FirstOrDefault();
            if(result != null)
            {
                result.FirstName=users.FirstName;
                result.LastName=users.LastName;
                result.Email=users.Email;
                result.UserName=users.UserName;
                result.Password=users.Password;
                result.DoB =users.DoB;
                result.Gender=users.Gender;
                result.PhoneNumber=users.PhoneNumber;
                res = db.SaveChanges();
            }
            return res;
        }
        // Set password reset token for the user
        public void SetPasswordResetToken(string userName, string token)
        {
            var user = db.user.Where(u => u.UserName == userName).FirstOrDefault();
            if (user != null)
            {
                user.PasswordResetToken = token;  // Add this field to your Users model
                user.ResetTokenExpiry = DateTime.Now.AddHours(1); // Token valid for 1 hour
                db.SaveChanges();
            }
        }
        public Users GetUserByPasswordResetToken(string token)
        {
            return db.user
                .Where(u => u.PasswordResetToken == token && u.ResetTokenExpiry > DateTime.Now)
                .FirstOrDefault();
        }
    }
}
