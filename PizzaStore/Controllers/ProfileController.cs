using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;

namespace PizzaStore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _UserManager;
        public ProfileController(PizzaStoreContext context, UserManager<PizzaStoreUser> UserManager)
        {
            _context = context;
            _UserManager = UserManager;

        }
        public IActionResult Index()
        {
            string UserId = _UserManager.GetUserId(User);

            var user = _context.Users.Find(UserId);
            return View(user);
        }
    }
}
