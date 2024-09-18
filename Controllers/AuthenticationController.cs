using Logins.Abstraction;
using Logins.Models;
using Logins.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Logins.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IConfigurationServices _configurationServices;

        public AuthenticationController(
            IUserServices userServices,
            IConfigurationServices configurationServices
            )
        {
            _userServices = userServices;
            _configurationServices = configurationServices;

        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User user)
        {
            _userServices.Register(user);
            return RedirectToAction("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            User? user = _userServices.GetUser(model.UserName, model.Password);
            if (user != null)
            {
                int expirytime = _configurationServices.GetExpiryTime();

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, model.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.GivenName,user.Name),
                    new Claim(ClaimTypes.Sid,user.Id.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = model.KeepLoggedIn,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(expirytime)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Home");
            }
            ViewData["validateMessage"] = "User not found";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return  RedirectToAction("Login", "Authentication");
        }
    }
}
