using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Data;
using PizzaStore.Models;
using PizzaStore.Utilities;
using System.Net.Mail;
using System.Net;

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

            double total = 0;
            foreach(CartItem item in cart.Items)
            {
                total += item.Product.Price * item.Quantity;
            }

            total = (total*0.2) + total;

            if (cart != null)
            {
                var order = new Order
                {
                    OrderNumber = RandomStringGenerator.GenerateRandomString(10),
                    CreatedDate = DateTime.Now,
                    OrderStatus = "Pending",
                    Total = Math.Round(total),
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
                var userId = _user.GetUserId(this.User);
                var current = _user.FindByIdAsync(userId).Result;
                var email = current.Email;
                var subject = "Order Confirmed";

                var tablebody = string.Empty;
                foreach (var item in cart.Items)
                {
                   tablebody += $" <tr>\r\n<td>{item.Product.Name}</td>\r\n <td>{item.Quantity}</td>\r\n<td>{item.Product.Price * item.Quantity}</td>\r\n</tr>";
                }

                var content = $"<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\"\r\n    xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <title></title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Roboto:400,600\" rel=\"stylesheet\" type=\"text/css\">\r\n    <style>\r\n    th, td {{\r\n            padding-inline: 20px;\r\n            padding-block: 5px;\r\n        }}    html,\r\n        body {{\r\n            margin: 0 auto !important;\r\n            padding: 0 !important;\r\n            height: 100% !important;\r\n            width: 100% !important;\r\n            font-family: 'Roboto', sans-serif !important;\r\n            font-size: 14px;\r\n            margin-bottom: 10px;\r\n            line-height: 24px;\r\n            color: #8094ae;\r\n            font-weight: 400;\r\n\r\n        }}\r\n\r\n        * {{\r\n            -ms-text-size-adjust: 100%;\r\n            -webkit-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n\r\n        table,\r\n        td {{\r\n\r\n            mso-table-lspace: 0pt !important;\r\n            mso-table-rspace: 0pt !important;\r\n\r\n        }}\r\n\r\n        table {{\r\n\r\n            border-spacing: 0 !important;\r\n            border-collapse: collapse !important;\r\n            table-layout: fixed !important;\r\n            margin: 0 auto !important;\r\n        }}\r\n\r\n\r\n        table table table {{\r\n\r\n            table-layout: auto;\r\n        }}\r\n\r\n\r\n        a {{\r\n\r\n            text-decoration: none;\r\n\r\n        }}\r\n\r\n        img {{\r\n\r\n            -ms-interpolation-mode: bicubic;\r\n        }}\r\n    </style>\r\n</head>\r\n\r\n<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color:\r\n    #f5f6fa;\">\r\n    <center style=\"width: 100%; background-color: #f5f6fa;\">\r\n        <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#f5f6fa\">\r\n            <tr>\r\n                <td style=\"padding: 40px 0;\">\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding-bottom:25px\">\r\n                                    <p style=\"font-size: 14px; color: #6576ff; padding-top: 12px;\">PIzza Shop</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;background-color:#ffffff;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align:center;padding: 30px 30px 20px\">\r\n                                    <h5 style=\"margin-bottom: 24px; color: #526484; font-size: 20px; font-weight: 400;\r\n                                        line-height: 28px;\">Order Confirmed</h5>\r\n                                    <p style=\"margin-bottom: 15px; color: #526484; font-size: 16px;\">Summary of your order</p>\r\n                                    <table border=\"1\" style=\"text-align: center;\">\r\n                                        <thead>\r\n                                            <tr>\r\n                                                <th>Name</th>\r\n                                                <th>Quantity</th>\r\n                                                <th>Total</th>\r\n                                            </tr>\r\n                                        </thead>\r\n                                        <tbody>\r\n                                       {tablebody}    \r\n                                            <tr>\r\n                                                <td colspan=\"2\">Total TTC</td>\r\n                                                <td>{total}</td>\r\n                                            </tr>\r\n                                        </tbody>\r\n                                    </table>\r\n                       <br><br>             <p style=\"margin-bottom: 15px;\">If you did not make this request, please contact\r\n                                        us or ignore this message.<br> This is an automatically generated email please do\r\n                                        not reply to this email.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding:25px 20px 0;\">\r\n                                    <p style=\"font-size: 13px;\">Copyright © 2020 Pizza Store. All rights reserved.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </center>\r\n</body>\r\n\r\n</html>";

                //await _sender.SendEmailAsync(email, subject, content);

                var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("0f3cbbb03fcfdd", "3bede6d5a81fc2"),
                    EnableSsl = true
                };

                var message = new MailMessage("pizzastore@gmail.com", email, subject, content);
                message.IsBodyHtml = true;
                client.Send(message);

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
