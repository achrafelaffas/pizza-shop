using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;
using PizzaStore.Models;
using PizzaStore.Utilities;

namespace PizzaStore.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _user;
        private SignInManager<PizzaStoreUser> _signInManager;
        public CheckOutController(PizzaStoreContext context, UserManager<PizzaStoreUser> user, SignInManager<PizzaStoreUser> signInManager)
        {
            _context = context;
            _user = user;
            _signInManager = signInManager;

        }
        public IActionResult Index() { 
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Index(Payement payement)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");

            if (!ModelState.IsValid)
            {
                return View(payement);
            }

            if (cart != null)
            {
                var order = new Order
                {
                    OrderNumber = RandomStringGenerator.GenerateRandomString(10),
                    CreatedDate = DateTime.Now,
                    OrderStatus = "Pending",
                    Total = 828,
                    UserId = _user.GetUserId(this.User)
                };

                _context.orders.Add(order);
                _context.SaveChanges();
                foreach (var item in cart.Items)
                {
                    Details detail = new Details();
                    detail.ProductId = item.Product.Id;
                    detail.price = item.Product.Price * item.Quantity;
                    detail.ts = DateTime.Now;
                    detail.OrderId = order.Id;
                    detail.quantity = item.Quantity;
                    _context.details.Add(detail);
                    _context.SaveChanges();
                }

                HttpContext.Session.Remove("Cart");
                return RedirectToAction("Index", "Orders");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
