// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PizzaStore.Areas.Identity.Data;
using PizzaStore.Models;

namespace PizzaStore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<PizzaStoreUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<PizzaStoreUser> userManager, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);


            var subject = "Confirm your email";
            var content = $"<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <title></title>\r\n    \r\n    <link href=\"https://fonts.googleapis.com/css?family=Roboto:400,600\" rel=\"stylesheet\" type=\"text/css\">\r\n    <style>\r\n        html,\r\n        body {{\r\n            margin: 0 auto !important;\r\n            padding: 0 !important;\r\n            height: 100% !important;\r\n            width: 100% !important;\r\n            font-family: 'Roboto', sans-serif !important;\r\n            font-size: 14px;\r\n            margin-bottom: 10px;\r\n            line-height: 24px;\r\n            color:#8094ae;\r\n            font-weight: 400;\r\n        }}\r\n        * {{\r\n            -ms-text-size-adjust: 100%;\r\n            -webkit-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n        table,\r\n        td {{\r\n            mso-table-lspace: 0pt !important;\r\n            mso-table-rspace: 0pt !important;\r\n        }}\r\n        table {{\r\n            border-spacing: 0 !important;\r\n            border-collapse: collapse !important;\r\n            table-layout: fixed !important;\r\n            margin: 0 auto !important;\r\n        }}\r\n        table table table {{\r\n            table-layout: auto;\r\n        }}\r\n        a {{\r\n            text-decoration: none;\r\n        }}\r\n        img {{\r\n            -ms-interpolation-mode:bicubic;\r\n        }}\r\n    </style>\r\n\r\n</head>\r\n\r\n<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f5f6fa;\">\r\n\t<center style=\"width: 100%; background-color: #f5f6fa;\">\r\n        <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#f5f6fa\">\r\n            <tr>\r\n               <td style=\"padding: 40px 0;\">\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding-bottom:25px\">\r\n                                    <p style=\"font-size: 14px; color: #6576ff; padding-top: 12px;\">PIzza Shop</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;background-color:#ffffff;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align:center;padding: 30px 30px 20px\">\r\n                                    <h5 style=\"margin-bottom: 24px; color: #526484; font-size: 20px; font-weight: 400; line-height: 28px;\">Hi!<br>Please click to confirm you email</h5>\r\n                                    <p style=\"margin-bottom: 15px; color: #526484; font-size: 16px;\">By clicking this button you will confirm your email to access your account</p>\r\n\r\n                                    <a href=\"{EmailConfirmationUrl}\" style=\"background-color:#6576ff;border-radius:4px;color:#ffffff;display:inline-block;font-size:13px;font-weight:600;line-height:44px;text-align:center;text-decoration:none;text-transform: uppercase; padding: 0 30px\">Confirm Email</a>\r\n                                    <p style=\"margin-bottom: 15px;\">If you did not make this request, please contact us or ignore this message.\r\n\r\n                                        This is an automatically generated email please do not reply to this email.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding:25px 20px 0;\">\r\n                                    <p style=\"font-size: 13px;\">Copyright © 2020 DashLite. All rights reserved.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n               </td>\r\n            </tr>\r\n        </table>\r\n    </center>\r\n</body>\r\n</html>";

            //await _sender.SendEmailAsync(email, subject, content);

            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("0f3cbbb03fcfdd", "3bede6d5a81fc2"),
                EnableSsl = true
            };

            var message = new MailMessage("pizzastore@gmail.com", email, subject, content);
            message.IsBodyHtml = true;
            client.Send(message);


            return Page();
        }
    }
}
