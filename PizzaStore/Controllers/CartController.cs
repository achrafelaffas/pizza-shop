using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;
using PizzaStore.Models;
using PizzaStore.Utilities;

namespace PizzaStore.Controllers
{
    public class CartController : Controller
    {
        private readonly PizzaStoreContext _context;
        private readonly UserManager<PizzaStoreUser> _user;
        private SignInManager<PizzaStoreUser> _signInManager;

        public CartController(PizzaStoreContext context, UserManager<PizzaStoreUser> user, SignInManager<PizzaStoreUser> signInManager)
        {
            _context = context;
            _user = user;
            _signInManager = signInManager;

        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            return View(cart);
        }

        public IActionResult ClearCart ()
        {
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        public IActionResult AddToCart(int Id)
        {

            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            
            var product = _context.products.Find(Id);
            if (product != null)
            {
                CartItem item = new CartItem();
                item.Product = product;
                cart.AddItem(item);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return Json(cart);
        }

        //public IActionResult ModifyQte(int cartValue, int productId)
        //{
        //    var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
        //    if (cart == null) {
        //        return RedirectToAction("index");
        //    }
        //    var cartItem = cart.Items.FirstOrDefault(x => x.Product.Id == productId);
        //    if (cartItem != null)
        //    {
        //        cart.Items.Remove(cartItem);
        //        cartItem.Quantity = cartValue;
        //        cart.Items.Add(cartItem);
        //        HttpContext.Session.SetObjectAsJson("Cart", cart);
        //    }

        //    return RedirectToAction("Index");
        //}


        public IActionResult RemoveOne(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null)
            {
                return RedirectToAction("index");
            }
            var cartItem = cart.Items.FirstOrDefault(x => x.Product.Id == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                cartItem.Quantity--;
                cart.Items.Add(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }


        public IActionResult AddOne(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart");
            if (cart == null)
            {
                return RedirectToAction("index");
            }
            var cartItem = cart.Items.FirstOrDefault(x => x.Product.Id == productId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                cartItem.Quantity++;
                cart.Items.Add(cartItem);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<Cart>("Cart") ?? new Cart();
            // Remove the item with the specified productId from the cart
            var item = cart.Items.Find(x => x.Product.Id == id);
            if (item != null)
            {
                cart.Items.Remove(item);
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
    }
}

