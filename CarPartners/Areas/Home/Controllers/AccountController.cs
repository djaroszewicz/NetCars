using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCars.Areas.Home.Models.Db.Account;
using NetCars.Areas.Home.Models.View.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NetCars.Areas.Home.Controllers
{
    [Area("home")]
    [Route("home/{controller}/{action=login}/{Id?}")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterView result)
        {
            if (ModelState.IsValid)
            {
                var user = new User() { Email = result.Email, UserName = result.Login };
                var register = await _userManager.CreateAsync(user, result.Password);

                if (register.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Home" });
                }

                foreach (var error in register.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { Area = "Home" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView result)
        {
            if (ModelState.IsValid)
            {
                var login = await _signInManager.PasswordSignInAsync(result.Login, result.Password, result.RemberMe, false);

                if (login.Succeeded)
                    return RedirectToAction("Index", "Home", new { Area = "Home" });

                ModelState.AddModelError("", "Nieprawidłowa próba logowania!");
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "Home" });
        }
    }
}
