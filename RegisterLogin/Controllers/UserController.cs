using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegisterLogin.Models;
using RegisterLogin.Services;

namespace RegisterLogin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices service;
        public UserController(IUserServices service)
        {
            this.service = service;
        }

        // GET: UserController
        public ActionResult Index()
        {
            ViewData["HideHeroSection"] = true; // Hide hero section on the Privacy page
            return View(service.GetAllUsers());
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            ViewData["HideHeroSection"] = true;
            var result = service.GetUsersById(id);  
            return View(result);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            ViewData["HideHeroSection"] = true;
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            if (!service.IsUserNameUnique(users.UserName))
            {
                ModelState.AddModelError("UserName", "Username is already taken.");
                return View(users);
            }

            try
            {
                int result = service.AddUsers(users);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["HideHeroSection"] = true;
            var result = service.GetUsersById(id);
            return View(result);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            try
            {
                int result = service.UpdateUsers(users);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
               
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            ViewData["HideHeroSection"] = true;
            var result = service.GetUsersById(id);
            return View(result);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            try
            {
                var result = service.DeleteUsers(id);
                if (result >= 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else 
                { 
                    return View();
                }
                
            }
            catch
            {
                return View();
            }
        }
        public IActionResult Login()
        {
            ViewData["HideHeroSection"] = true; // Hide hero section on the Privacy page
            return View();
        }
        [HttpPost]
        public IActionResult Login(Users users)
        {
            try
            {
                var result = service.Login(users);
                if (result != null)
                {
                    return RedirectToAction(nameof(UserProfile), new { UserName = users.UserName });
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
        // UserProfile Action
        public IActionResult UserProfile([FromQuery] string UserName)
        {
            var user = service.GetUsers(UserName);

            if (user == null)
            {
                ViewData["HideHeroSection"] = true;
                ViewBag.ErrorMessage = "User not found";
                return View("UserNotFound");  // You can create a specific view for user not found.
            }

            // Optionally, pass the profile photo if it exists
            var profilePhotoPath = $"/images/{user.UserName}.jpg"; // Assuming photo is saved as the username
            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{user.UserName}.jpg")))
            {
                ViewData["HideHeroSection"] = true;
                ViewData["ProfilePhoto"] = profilePhotoPath;
            }

            return View(user);
        }

        // Upload Profile Photo Action
        [HttpPost]
        public IActionResult UploadProfilePhoto(IFormFile profilePhoto, string userName)
        {

            var user = service.GetUsers(userName);

            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found";
                return RedirectToAction(nameof(UserProfile), new { UserName = userName });
            }

            if (profilePhoto != null && profilePhoto.Length > 0)
            {
                // Path to save the uploaded file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", $"{userName}.jpg");

                // Save the file locally
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    profilePhoto.CopyTo(stream);
                }

                // Redirect back to UserProfile page with the uploaded photo
                return RedirectToAction(nameof(UserProfile), new { UserName = userName });
            }

            // If no photo is uploaded, show an appropriate message
            ViewBag.ErrorMessage = "Please select a valid photo to upload.";
            return RedirectToAction(nameof(UserProfile), new { UserName = userName });
        }

        [HttpPost]
        public JsonResult IsUserNameUnique(string userName)
        {
            bool isUnique = service.IsUserNameUnique(userName);
            return Json(new { isUnique });
        }
        
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string userName)
        {
            service.SendPasswordResetEmail(userName);
            ViewBag.Message = "If the username exists, a password reset link has been sent to the registered email.";
            return View();
        }

        // Reset password view
        public IActionResult ResetPassword(string token)
        {
            var user = service.GetUserByPasswordResetToken(token);
            if (user == null)
            {
                ViewBag.Error = "Invalid or expired token.";
                return View("Error");
            }

            return View(new ResetPasswordModel { Token = token });
        }
        [HttpGet("ResetPassword")]
        public IActionResult ResetPasswords(string token)
        {
            var user = service.GetUserByPasswordResetToken(token);
            if (user == null)
            {
                ViewBag.Error = "Invalid or expired token.";
                return View("Error");
            }

            return View(new ResetPasswordModel { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                int result = service.ResetPassword(model.Token, model.NewPassword);
                if (result > 0)
                {
                    ViewBag.Message = "Password has been reset successfully.";
                    return View("Login");
                }
                else
                {
                    ViewBag.Error = "Failed to reset the password.";
                }
            }
            return View(model);
        }
    }
}




