using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;
using PizzaStore.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace PizzaStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<PizzaStoreUser> userManager, PizzaStoreContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<PizzaStoreUser> users = _context.Users;
            foreach (PizzaStoreUser user in users)
            {
                if (user.Id == _userManager.GetUserId(this.User))
                {
                    HttpContext.Session.SetString("Username", user.Lastname + " " + user.Firstname);
                    Console.WriteLine(HttpContext.Session.GetString("Username"));
                    
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}