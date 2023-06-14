// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using PizzaStore.Areas.Identity.Data;

namespace PizzaStore.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<PizzaStoreUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<PizzaStoreUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var subject = "Reset your password";
                var content = $"<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\"\r\n    xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <title></title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Roboto:400,600\" rel=\"stylesheet\" type=\"text/css\">\r\n    <style>\r\n        html,\r\n        body {{\r\n            margin: 0 auto !important;\r\n            padding: 0 !important;\r\n            height: 100% !important;\r\n            width: 100% !important;\r\n            font-family: 'Roboto', sans-serif !important;\r\n            font-size: 14px;\r\n            margin-bottom: 10px;\r\n            line-height: 24px;\r\n            color: #8094ae;\r\n            font-weight: 400;\r\n\r\n        }}\r\n\r\n        * {{\r\n            -ms-text-size-adjust: 100%;\r\n            -webkit-text-size-adjust: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n\r\n        table,\r\n        td {{\r\n\r\n            mso-table-lspace: 0pt !important;\r\n            mso-table-rspace: 0pt !important;\r\n\r\n        }}\r\n\r\n        table {{\r\n\r\n            border-spacing: 0 !important;\r\n            border-collapse: collapse !important;\r\n            table-layout: fixed !important;\r\n            margin: 0 auto !important;\r\n        }}\r\n\r\n\r\n        table table table {{\r\n\r\n            table-layout: auto;\r\n        }}\r\n\r\n\r\n        a {{\r\n\r\n            text-decoration: none;\r\n\r\n        }}\r\n\r\n        img {{\r\n\r\n            -ms-interpolation-mode: bicubic;\r\n        }}\r\n\r\n        th, td {{\r\n            padding-inline: 20px;\r\n            padding-block: 10px;\r\n        }}\r\n    </style>\r\n</head>\r\n\r\n<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color:\r\n    #f5f6fa;\">\r\n    <center style=\"width: 100%; background-color: #f5f6fa;\">\r\n        <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" bgcolor=\"#f5f6fa\">\r\n            <tr>\r\n                <td style=\"padding: 40px 0;\">\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding-bottom:25px\">\r\n                                    <p style=\"font-size: 14px; color: #6576ff; padding-top: 12px;\">PIzza Shop</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;background-color:#ffffff;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align:center;padding: 30px 30px 20px\">\r\n                                    <h5 style=\"margin-bottom: 24px; color: #526484; font-size: 20px; font-weight: 400;\r\n                                        line-height: 28px;\">Forgot Password</h5>\r\n                                    <p style=\"margin-bottom: 15px; color: #526484; font-size: 16px;\">Reset your account's password</p>\r\n                                    <p>To reset your password, click on the reset password below</p>\r\n                                    <br>\r\n                                    <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reset Password</a>\r\n                                    <br>\r\n                                    <p style=\"margin-bottom: 15px;\">If you did not make this request, please contact\r\n                                        us or ignore this message.<br> This is an automatically generated email please do\r\n                                        not reply to this email.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                    <table style=\"width:100%;max-width:620px;margin:0 auto;\">\r\n                        <tbody>\r\n                            <tr>\r\n                                <td style=\"text-align: center; padding:25px 20px 0;\">\r\n                                    <p style=\"font-size: 13px;\">Copyright © 2023 Pizza Store. All rights reserved.</p>\r\n                                </td>\r\n                            </tr>\r\n                        </tbody>\r\n                    </table>\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </center>\r\n</body>\r\n\r\n</html>";

                var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("0f3cbbb03fcfdd", "3bede6d5a81fc2"),
                    EnableSsl = true
                };

                var message = new MailMessage("pizzastore@gmail.com", Input.Email, subject, content);
                message.IsBodyHtml = true;
                client.Send(message);


                //await _emailSender.SendEmailAsync(
                //    Input.Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
