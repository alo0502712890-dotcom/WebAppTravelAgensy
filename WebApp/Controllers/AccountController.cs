using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebApp.DB;
using WebApp.Entity;
using WebApp.Helpers;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AgencyDBContext _agencyDBContext;
        private UserModel _userModel;

        public AccountController(AgencyDBContext agencyDBContext)
        {
            _agencyDBContext = agencyDBContext;
            _userModel = new UserModel(agencyDBContext);
        }


        // -------------------------------
        // LOGIN / LOGOUT
        // -------------------------------
        [HttpGet]
        public IActionResult Index() => RedirectToAction("LoginIn");

        [HttpGet]
        public IActionResult LoginIn()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public IActionResult CheckUser(string email, string password)
        {
            var user = _userModel.GetUserByEmail(email);

            if (user != null && SecurePasswordHasher.Verify(password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim("Login", user.Login)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Admin");
            }

            return View("LoginIn", new ErrorViewModel { ErrorMessage = "User or Password incorrect" });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("LoginIn");
        }


        // -------------------------------
        // REGISTRATION
        // -------------------------------
        [HttpGet]
        public IActionResult RegisterIn() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            // Валідація моделі
            if (!ModelState.IsValid)
            {
                var validationError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .FirstOrDefault(e => !string.IsNullOrEmpty(e.ErrorMessage));

                if (validationError != null)
                    model.ErrorMessage = validationError.ErrorMessage;

                return View("RegisterIn", model);
            }

            // Перевірка погодження з умовами
            if (model.Check != "1")
            {
                model.ErrorMessage = "Ви повинні погодитися з Умовами та Положеннями.";
                return View("RegisterIn", model);
            }

            // Перевірка унікальності email
            if (_userModel.GetUserByEmail(model.User_Email) != null)
            {
                model.ErrorMessage = "Користувач з таким Email вже існує.";
                return View("RegisterIn", model);
            }

            // Створення нового користувача
            var newUser = new User
            {
                Email = model.User_Email,
                Login = model.User_Login,
                PasswordHash = SecurePasswordHasher.Hash(model.User_Password),
                DateOfCreated = DateTime.Now
            };

            if (await _userModel.AddUserAsync(newUser))
            {
                // Авторизація нового користувача
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, newUser.Email),
                    new Claim("Login", newUser.Login)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Admin");
            }

            model.ErrorMessage = "Помилка при збереженні даних. Спробуйте пізніше.";
            return View("RegisterIn", model);
        }


        // -------------------------------
        // GOOGLE AUTHENTICATION
        // -------------------------------
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded) return RedirectToAction("LoginIn");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            var user = _userModel.GetUserByEmail(email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Login = name,
                    PasswordHash = "", // не має пароля
                    DateOfCreated = DateTime.Now
                };
                await _userModel.AddUserAsync(user);
            }

            // Авторизація через cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("Login", user.Login)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Admin");
        }

        // -------------------------------
        // FORGOT / RESET PASSWORD
        // -------------------------------
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = _userModel.GetUserByEmail(email);
            if (user == null)
            {
                ViewBag.Error = "Користувача з таким email не знайдено";
                return View();
            }

            // Генерація токена
            string token = Guid.NewGuid().ToString();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);
            await _agencyDBContext.SaveChangesAsync();

            // Лінк на відновлення
            string resetLink = Url.Action("ResetPassword", "Account",
                new { token = token, email = email }, Request.Scheme);

            // Надсилаємо email
            await EmailSender.SendEmailAsync(email, "Відновлення пароля",
                $"Перейдіть за посиланням для відновлення: <a href='{resetLink}'>Reset Password</a>");

            ViewBag.Message = "Лист для відновлення пароля надіслано!";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) =>
            View(new ResetPasswordViewModel { Token = token, Email = email });

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = _userModel.GetUserByEmail(model.Email);

            if (user == null || user.ResetToken != model.Token || user.ResetTokenExpiry < DateTime.Now)
            {
                ViewBag.Error = "Невірний або прострочений токен";
                return View();
            }

            user.PasswordHash = SecurePasswordHasher.Hash(model.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            await _agencyDBContext.SaveChangesAsync();

            return RedirectToAction("LoginIn");
        }
    }
}
