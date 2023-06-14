using Microsoft.AspNetCore.Mvc;

namespace PizzaStore.Models
{
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
