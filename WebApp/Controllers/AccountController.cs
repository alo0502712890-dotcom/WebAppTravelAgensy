using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entity;
using WebApp.Helpers;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // -------------------------
        // LOGIN
        // -------------------------
        [HttpGet]
        public async Task<IActionResult> LoginIn()
        {

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LoginIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            User? user = null;

            // визначаємо: email чи телефон
            var isEmail = model.EmailOrPhone.Contains("@");

            if (isEmail)
            {
                user = await _userManager.FindByEmailAsync(model.EmailOrPhone);

                if (user == null)
                {
                    ModelState.AddModelError("", "Користувач не знайдений");
                    return View(model);
                }

                // підтвердження email ТІЛЬКИ для email-логіну
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Підтвердіть email перед входом");
                    return View(model);
                }
            }
            else
            {
                // логін по телефону
                user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == model.EmailOrPhone);

                if (user == null)
                {
                    ModelState.AddModelError("", "Користувач з таким телефоном не знайдений");
                    return View(model);
                }
            }

            // логін через UserName
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Невірний логін або пароль");
                return View(model);
            }

            // редірект по ролі
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }


        // -------------------------
        // REGISTER (Email)
        // -------------------------
        [HttpGet]
        public IActionResult RegisterIn() => View();

        [HttpPost]
        public async Task<IActionResult> RegisterIn(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Користувач з таким Email вже існує");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Login = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "User");

            // 🔑 EMAIL CONFIRMATION TOKEN
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token },
                Request.Scheme
            );

            await EmailSender.SendEmailAsync(
                user.Email,
                "Підтвердження реєстрації",
                $"""
                <h3>Підтвердження email</h3>
                <p>Для завершення реєстрації натисніть:</p>
                <a href="{confirmLink}">Підтвердити email</a>
                """
            );

            return RedirectToAction(nameof(EmailConfirmationSent));
        }

        [HttpGet]
        public IActionResult EmailConfirmationSent()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return RedirectToAction(nameof(LoginIn));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return View("Error");

            return View("EmailConfirmed");
        }


        // -------------------------
        // GOOGLE LOGIN
        // -------------------------
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(LoginIn));

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(LoginIn));

            var user = await _userManager.FindByEmailAsync(email);

            // 🔐 Адмінів через Google НЕ пускаємо
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _signInManager.SignOutAsync();
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                TempData["AuthError"] = "Адміністратори не можуть входити через Google";
                return RedirectToAction(nameof(LoginIn));
            }

            // ✅ Якщо користувач вже існує → логін
            if (user != null)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(AfterLogin));
            }

            // ❗ Користувача нема → підтвердження
            TempData["GoogleEmail"] = email;
            TempData["GoogleProvider"] = info.LoginProvider;
            TempData["GoogleKey"] = info.ProviderKey;

            return RedirectToAction(nameof(ConfirmGoogle));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmGoogle()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                // ❌ нема активного Google-логіну
                return RedirectToAction(nameof(LoginIn));
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(LoginIn));

            return View(new ConfirmGoogleViewModel
            {
                Email = email
            });
        }



        [HttpPost]
        public async Task<IActionResult> ConfirmGoogle(ConfirmGoogleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // захист від повторної реєстрації
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return RedirectToAction(nameof(LoginIn));

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Login = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "User");

            // привʼязуємо Google
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null)
                await _userManager.AddLoginAsync(user, info);

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(AfterLogin));
        }


        [Authorize]
        public async Task<IActionResult> AfterLogin()
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "Home");
        }

        // -------------------------
        // RegisterByPhone
        // -------------------------
        [HttpGet]
        public IActionResult RegisterByPhone()
        {
            return View(new RegisterByPhoneViewModel());
        }

        [HttpPost]
        public IActionResult RegisterByPhone(RegisterByPhoneViewModel model)
        {
            Console.WriteLine($"[POST] IsCodeSent={model.IsCodeSent}, Phone={model.PhoneNumber}, Code={model.Code}");

            // STEP 1
            if (!model.IsCodeSent)
            {
                if (string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    ModelState.AddModelError(nameof(model.PhoneNumber), "Введіть телефон");
                    return View(model);
                }

                var code = "1234";
                HttpContext.Session.SetString("PhoneCode", code);
                HttpContext.Session.SetString("PhoneNumber", model.PhoneNumber);

                Console.WriteLine($"📱 FAKE SMS to {model.PhoneNumber}: {code}");

                model.IsCodeSent = true;
                return View(model);
            }

            // STEP 2
            var savedCode = HttpContext.Session.GetString("PhoneCode");
            var phone = HttpContext.Session.GetString("PhoneNumber");

            Console.WriteLine($"[SESSION] savedCode={savedCode}, phone={phone}");

            if (string.IsNullOrEmpty(savedCode) || string.IsNullOrEmpty(phone))
            {
                ModelState.AddModelError("", "Сесія втрачена. Спробуйте ще раз (Надіслати код).");
                model.IsCodeSent = false;
                return View(model);
            }

            if (model.Code != savedCode)
            {
                ModelState.AddModelError("", "Невірний код");
                model.IsCodeSent = true;
                return View(model);
            }

            // OK -> redirect
            HttpContext.Session.Remove("PhoneCode");
            TempData["PhoneNumber"] = phone;

            Console.WriteLine("[OK] Redirect -> FinishPhoneRegistration");
            return RedirectToAction(nameof(FinishPhoneRegistration));
        }

        [HttpGet]
        public IActionResult FinishPhoneRegistration()
        {
            var phone = TempData["PhoneNumber"]?.ToString();
            if (string.IsNullOrEmpty(phone))
                return RedirectToAction(nameof(RegisterByPhone));

            TempData.Keep("PhoneNumber"); // ✅ важливо

            return View(new FinishPhoneRegistrationViewModel
            {
                PhoneNumber = phone
            });
        }


        [HttpPost]
        public async Task<IActionResult> FinishPhoneRegistration(
    FinishPhoneRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 🔒 перевірка: телефон уже існує
            var exists = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == model.PhoneNumber);

            if (exists)
            {
                ModelState.AddModelError("", "Користувач з таким номером телефону вже існує");
                return View(model);
            }

            var user = new User
            {
                UserName = model.PhoneNumber,
                PhoneNumber = model.PhoneNumber,
                Email = $"{model.PhoneNumber}@phone.local",
                Login = model.Login
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }



        // -------------------------
        // LOGOUT
        // -------------------------
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //----------------------------
        //Profile
        //----------------------------

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("LoginIn");
            }

            var model = new ProfileViewModel
            {
                Login = user.Login,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View("Dashboard/Profile", model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("LoginIn");

            // Profile data
            user.Login = model.Login;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);

            // Password change (only if filled)
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(
                    user,
                    model.CurrentPassword,
                    model.NewPassword
                );

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
                TempData["PasswordChanged"] = true;
                
            }
            TempData["ProfileUpdated"] = true;
            TempData["Success"] = "Дані збережено";
            return RedirectToAction("Profile");
        }


        [Authorize]
        public IActionResult Inbox()
        {
            return View("Dashboard/Inbox");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Error = "Введіть email";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            // ❗ Завжди однакова відповідь (security best practice)
            if (user == null)
            {
                ViewBag.Message = "Якщо email існує — лист надіслано";
                return View();
            }

            // ❗ Google-акаунт без пароля
            if (!await _userManager.HasPasswordAsync(user))
            {
                ViewBag.Error = "Цей акаунт використовує Google. Увійдіть через Google.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action(
                "ResetPassword",
                "Account",
                new { token, email },
                Request.Scheme
            );

            await EmailSender.SendEmailAsync(
                email,
                "Відновлення пароля",
                $"""
        <h3>Відновлення пароля</h3>
        <p>Натисніть для зміни пароля:</p>
        <a href="{link}">Змінити пароль</a>
        """
            );

            ViewBag.Message = "Лист для відновлення надіслано";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return RedirectToAction(nameof(LoginIn));

            return View(new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            });
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ❗ ТІЛЬКИ ТАК
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Помилка відновлення пароля");
                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                model.Token,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction(nameof(LoginIn));
        }


    }
}
