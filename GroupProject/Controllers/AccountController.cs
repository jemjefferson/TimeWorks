using GroupProject.DataAccess;
using GroupProject.Models;
using GroupProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GroupProject.Controllers
{
    [Authorize]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly DataService _dataService;

        public AccountController(DataContext dataContext)
        {
            // Instantiate an instance of the data service.
            _dataService = new DataService(dataContext);
        }

        [AllowAnonymous]
        [HttpGet("sign-in")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Login user = _dataService.GetLogin(loginViewModel.UserName);
            Employee employee = _dataService.GetEmployee(user.EmployeeID);

            if (user == null)
            {
                // Set email address not registered error message.
                ModelState.AddModelError("Error", "An account does not exist with that email address.");

                return View();
            }

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            PasswordVerificationResult passwordVerificationResult =
                passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginViewModel.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                // Set invalid password error message.
                ModelState.AddModelError("Error", "Invalid password.");

                return View();
            }

            // Add the user's ID (NameIdentifier), first name and role
            // to the claims that will be put in the cookie.
            bool manager = _dataService.GetEmployees().Where(e => e.Manager == employee.Id).Count() > 0;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.FirstName + " " + employee.LastName),
                new Claim(ClaimTypes.Role, user.Admin ? "Admin" : manager ? "Management" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [Route("sign-out"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [Route("access-denied")]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("accountdropdown")]
        public ActionResult AccountDropdown()
        {
            return View();
        }

        [HttpGet("change-password"), Authorize]
        public IActionResult ChangePassword()
        {
            PasswordChangeViewModel pcvm = new PasswordChangeViewModel();

            return View(pcvm);
        }

        [HttpPost("change-password"), Authorize]
        public IActionResult ChangePassword(PasswordChangeViewModel pcvm)
        {
            if (!ModelState.IsValid)
            {
                pcvm = new PasswordChangeViewModel();

                return View(pcvm);
            }

            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Login login = _dataService.GetLogin(userId);

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            login.PasswordHash = passwordHasher.HashPassword(login.UserName, pcvm.Password);

            _dataService.UpdatePassword(login);

            return RedirectToAction("Logout", "Account");
        }
    }
}
