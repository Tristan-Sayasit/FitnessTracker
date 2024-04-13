using System.Runtime.CompilerServices;
using FitnessTracker.Contexts;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace FitnessTracker.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly MySqlConnection _db;

        public WorkoutController(MySqlConnection db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                var context = new UserContext(_db);
                var vm = new SplitsViewModel
                {
                    Splits = await context.GetUserSplits(HttpContext.Session.GetString("userid")!)
                };
                return View(vm);
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSplit(SplitModel body)
        {
            var context = new WorkoutContext(_db);
            body.user_id = HttpContext.Session.GetString("userid")!;
            body.split_id = Guid.NewGuid().ToString();
            await context.CreateSplit(body.user_id, body.split_name, body.split_id);
            var day = new DateOnly();
            for (int i = 0; i < 7; i++)
            {
                await context.CreateSplitDay(day, body.split_id);
                day = day.AddDays(1);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetSplitDaysFromSplit(string splitid)
        {
            var context = new WorkoutContext(_db);
            return Ok(await context.GetSplitDaysFromSplitID(splitid));
        } 
    }
}
