using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;
using PizzaStore.Models;
using System.Linq;
using System.Reflection.Metadata;

namespace PizzaStore.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _user;

        public OrderDetailsController(PizzaStoreContext context, UserManager<PizzaStoreUser> user)
        {
            _context = context;
            _user = user;
        }

        public IActionResult OrderDetails(int orderId)
        {
            var details = _context.details
        .Where(q => q.OrderId == orderId)
        .Join(
            _context.products,
            details => details.ProductId,
            product => product.Id,
            (details, product) => new DetailsProductViewModel { Details = details, Product = product }
        )
        .ToList();

            return View(details);
        }

        public IActionResult DeleteOrder(int orderId)
        {
            var order = _context.orders.Find(orderId);
            if (order != null)
            {
                _context.orders.Remove(order);
                var details = _context.details
                    .Where(q => q.OrderId == orderId);
                _context.SaveChanges();

                foreach(var item in details)
                {
                    _context.details.Remove(item);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index","Orders");
        }

    }
}
