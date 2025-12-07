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

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("LoginIn");
        }

        [HttpGet]
        public IActionResult LoginIn()
        {
            ModelState.Clear();
            return View();
        }


        [HttpPost]
        public IActionResult CheckUser(string email, string password)
        {

            User? user = _userModel.GetUserByEmail(email);
            // string passwordHash = SecurePasswordHasher.Hash(password);
            if (user != null && SecurePasswordHasher.Verify(password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim("Login", user.Login)
                };
                var identity = new ClaimsIdentity(claims, "CoockieAuth", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View("LoginIn", new ErrorViewModel() { ErrorMessage = "User or Password incorrect" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            // Валідація
            if (!ModelState.IsValid)
            {
                var validationError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .FirstOrDefault(e => !string.IsNullOrEmpty(e.ErrorMessage));

                if (validationError != null)
                {
                    model.ErrorMessage = validationError.ErrorMessage;
                }

                return View("RegisterIn", model);
            }

            // ПЕРЕВІРКА 2: ПРАПОРЕЦЬ "Я ПОГОДЖУЮСЬ" 
            if (model.Check != "1")
            {
                model.ErrorMessage = "Ви повинні погодитися з Умовами та Положеннями.";
                return View("RegisterIn", model);
            }

            // ПЕРЕВІРКА 3: УНІКАЛЬНІСТЬ EMAIL
            if (_userModel.GetUserByEmail(model.User_Email) != null)
            {
                model.ErrorMessage = "Користувач з таким Email вже існує.";
                return View("RegisterIn", model);
            }

            // Створення об'єкта User
            var newUser = new WebApp.Entity.User
            {
                Email = model.User_Email,
                Login = model.User_Login,
                PasswordHash = SecurePasswordHasher.Hash(model.User_Password), 
                DateOfCreated = DateTime.Now
            };

            // Додавання користувача в БД
            if (await _userModel.AddUserAsync(newUser))
            {
                // --- АВТОРИЗАЦІЯ ---
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, newUser.Email),
                    new Claim("Login", newUser.Login)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Ошибка при сохранении в БД
                model.ErrorMessage = "Помилка при збереженні даних. Спробуйте пізніше.";
                return View("RegisterIn", model);
            }
        }

        [HttpGet]
        public IActionResult RegisterIn()
        {
            return View(new RegisterViewModel());
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("LoginIn");
        }


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

            if (!result.Succeeded)
                return RedirectToAction("LoginIn");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);

            // Перевіряємо чи є юзер
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

            // Авторизація
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Login", user.Login)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Admin");
        }

    }
}
