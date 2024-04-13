using FitnessTracker.Contexts;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace FitnessTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly MySqlConnection _db;

        public HomeController(MySqlConnection db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromForm]LoginSignupViewModel loginSignup)
        {
            var context = new UserContext(_db);
            await context.CreateUser(loginSignup.UserName, loginSignup.Password);
            return RedirectToAction("Login", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromForm]LoginSignupViewModel loginSignupForm)
        {
            var context = new UserContext(_db);
            var potentialUser = await context.LoginUser(loginSignupForm.UserName, loginSignupForm.Password);
            if (potentialUser != null)
            {
                HttpContext.Session.SetString("username", loginSignupForm.UserName);
                HttpContext.Session.SetString("userid", potentialUser.user_id);
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Index()
        {
            var vm = new HomeViewModel()
            {
                LoginModel = new()
            };
            return RedirectToAction(HttpContext.Session.GetString("username") != null ? "Dashboard" : "Login");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var context = new UserContext(_db);
                var vm = new ProfileViewModel
                {
                    User = await context.GetUserByID(HttpContext.Session.GetString("userid")!)
                };
                return View(vm);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserModel body)
        {
            var context = new UserContext(_db);
            body.user_id = HttpContext.Session.GetString("userid")!;
            await context.UpdateUser(body);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}