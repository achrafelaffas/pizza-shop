using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;

namespace PizzaStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {

        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _UserManager;
        public OrdersController(PizzaStoreContext context, UserManager<PizzaStoreUser> UserManager)
        {
            _context = context;
            _UserManager = UserManager;

        }
        public async Task<IActionResult> Index()
        {
            string UserId = _UserManager.GetUserId(User);

            return _context.products != null ?
                           View(await _context.orders.Where(q => q.UserId == UserId).ToListAsync()) :
                           Problem("Entity set 'PizzaStoreContext.products'  is null.");
        }
    }
}
