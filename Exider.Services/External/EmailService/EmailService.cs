﻿using System.Net.Mail;
using System.Net;
using Exider.Dependencies.Services;
using Exider.Core;
using Exider.Core.Models.Account;

namespace Exider_Version_2._0._0.ServerApp.Services
{

    public class EmailService : IEmailService
    {

        private static readonly string _emailConfirmationHtml = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <meta name=\"google\" content=\"notranslate\">\r\n    <style>\r\n        @import url('https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;0,800;1,300;1,400;1,500;1,600;1,700;1,800&display=swap');\r\n\r\n        * {\r\n            margin: 0;\r\n            padding: 0;\r\n            box-sizing: border-box;\r\n        }\r\n\r\n        body {\r\n            margin-top: 100px;\r\n            font-family: 'Open Sans', sans-serif;\r\n            color: black;\r\n        }\r\n\r\n        .m-b {\r\n\r\n            margin-bottom: 20px;\r\n\r\n        }\r\n\r\n        h1 {\r\n\r\n            font-size: 28px;\r\n            color: black;\r\n\r\n        }\r\n\r\n        h2 {\r\n\r\n            color: black;\r\n\r\n        }\r\n\r\n        h3 {\r\n\r\n            font-size: 16px;\r\n            color: black;\r\n\r\n        }\r\n\r\n        .wrapper {\r\n            height: auto;\r\n            width: 450px;\r\n            grid-gap: 23px;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n            overflow: visible;\r\n        }\r\n\r\n        .content-wrapper {\r\n\r\n            padding-left: 30px;\r\n            padding-right: 30px;\r\n\r\n        }\r\n\r\n        .logo {\r\n            height: 70px;\r\n            width: 70px;\r\n            margin-bottom: 10px;\r\n            object-fit: contain;\r\n            user-select: none;\r\n        }\r\n\r\n        p {\r\n            text-align: justify;\r\n            font-weight: 500;\r\n            font-size: 14px;\r\n            color: #515151;\r\n        }\r\n\r\n        span {\r\n            color: #2f8fff;\r\n            cursor: pointer;\r\n            user-select: none;\r\n        }\r\n\r\n        a {\r\n            text-decoration: none;\r\n        }\r\n\r\n        .button {\r\n            background: #5096ff;\r\n            border-radius: 30px;\r\n            text-align: center;\r\n            padding: 15px 45px;\r\n            width: fit-content;\r\n            user-select: none;\r\n            font-weight: 600;\r\n            text-transform: uppercase;\r\n            text-decoration: none;\r\n            color: white;\r\n            outline: none;\r\n            border: none;\r\n            outline: none;\r\n            cursor: pointer;\r\n            font-size: 14px;\r\n            transition: 0.1s ease;\r\n        }\r\n\r\n        .code {\r\n            height: auto;\r\n            width: fit-content;\r\n            text-align: center;\r\n            padding: 10px 20px;\r\n            background: #f3f3f3;\r\n            margin-bottom: 20px;\r\n            border-radius: 10px;\r\n            font-size: 12px;\r\n        }\r\n\r\n        footer {\r\n            height: auto;\r\n            width: 100%;\r\n            padding: 20px 30px;\r\n            background: #f8f8f8;\r\n            color: #bdbdbd;\r\n            user-select: none;\r\n        }\r\n\r\n        .header {\r\n            border-bottom: 1px solid #e4e4e4;\r\n            padding-bottom: 4%;\r\n            font-size: 12px;\r\n            display: flex;\r\n        }\r\n\r\n        .links {\r\n            margin-left: auto;\r\n            display: flex;\r\n        }\r\n\r\n            .links a:hover {\r\n                text-decoration: underline;\r\n            }\r\n\r\n        footer a, footer p {\r\n            color: #bdbdbd;\r\n        }\r\n\r\n        .content {\r\n            margin-top: 20px;\r\n            display: flex;\r\n        }\r\n\r\n        .app {\r\n            height: 25px;\r\n            cursor: pointer;\r\n        }\r\n\r\n        @media (max-width: 550px) {\r\n\r\n            .wrapper {\r\n                width: 95vw;\r\n            }\r\n\r\n            footer {\r\n                padding: none;\r\n                font-size: 11px;\r\n            }\r\n\r\n            .header {\r\n                flex-wrap: wrap;\r\n            }\r\n\r\n            #not-necessary {\r\n                display: none;\r\n            }\r\n        }\r\n    </style>\r\n\r\n</head>\r\n<body>\r\n\r\n    <div class=\"wrapper\">\r\n        <div class=\"content-wrapper\">\r\n            <a href=\"https://exider.com\">\r\n                <img src=\"https://lh3.googleusercontent.com/u/0/drive-viewer/AEYmBYS-b_pMLiSYlDXWgyda29nFKjnMYUtiWpiwYhqwvDkihZBzsq1RAQGsR9cCBgmo8NOBuyg03EXGrxatGF8bD1mbNB0E0w=w1960-h2358\" alt=\"logo\" class=\"logo\" draggable=\"false\" />\r\n            </a>\r\n            <h1 class=\"m-b\">Successfully registered</h1>\r\n            <p class=\"m-b\">Thank you for choosing to register an account with us. To complete your registration, please <span>follow the link</span> below and enter the <span>confirmation code.</span></p>\r\n            <a href=\"https://www.youtube.com/\" style=\"width: fit-content; cursor: auto;\">\r\n                <div class=\"button m-b\" style=\"cursor: pointer;\">confirm email</div>\r\n            </a>\r\n            <p class=\"m-b\">You will be asked to enter <span>this code</span>,<br>\r\n            Pease <span>do not show</span> it to anyone</p>\r\n            <div class=\"code\">\r\n                <h2>CODE</h2>\r\n            </div>\r\n            <p class=\"m-b\">Exider helps you automatically sync files across your devices, share them with friends, and edit them using neural networks</p>\r\n            <h3 class=\"m-b\">Why did you receive this letter?</h3>\r\n            <p class=\"m-b\">An attempt was made to register an account using your email. If this is not you, then simply ignore this message and without email confirmation, the account will be automatically deleted after 7 days</p>\r\n        </div>\r\n        <footer>\r\n            <div class=\"header\">\r\n                <p style=\"font-size: 11px;\">© Andreev S, Minsk 2024</p>\r\n                <div class=\"links\">\r\n                    <a href=\"#\" style=\"margin-right: 20px; color: #bdbdbd;\">Terms of use</a>\r\n                    <a href=\"#\" style=\"margin-right: 20px; color: #bdbdbd;\">Privacy policy</a>\r\n                    <a href=\"#\" style=\"color: #bdbdbd;\" id=\"not-necessary\">Report a bug</a>\r\n                </div>\r\n            </div>\r\n            <div class=\"content\">\r\n                <div>\r\n                    <a href=\"#\">\r\n                        <img src=\"https://lh3.google.com/u/0/d/1-LMzx-NaJXtCJfuyrlozf2mOSidgB35e=w2000-h2178-iv1\" class=\"app\">\r\n                    </a>\r\n                    <a href=\"#\">\r\n                        <img src=\"https://lh3.google.com/u/0/d/16axLd8CivfQuwt5MZ03zYNjm3L61YmXq=w2000-h2178-iv1\" class=\"app\">\r\n                    </a>\r\n                    <h4 style=\"margin-top: 10px; font-size: 14px;\">Contact us on social networks</h4>\r\n                    <p style=\"margin-top: 5px; font-size: 12px; margin-right: 30px;\">If you have any suggestions about our service please let us know on social networks</p>\r\n                </div>\r\n                <div class=\"links\">\r\n                    <a href=\"https://www.facebook.com/profile.php/?id=61553222136930&paipv=0&eav=AfYg6dkHp8U4FG3lqDfucLJeTJhV-2BSs3BHd1zuu6Jv5qQShEmPZRbBFigDzrt5DA4&_rdr\">\r\n                        <img src=\"https://lh3.google.com/u/0/d/1UdJ7yljnnkA5ksJZ4j-B5g0p0Dca40LY=w2000-h2408-iv1\" style=\"margin-right: 20px;\" class=\"app\">\r\n                    </a>\r\n                    <a href=\"https://instagram.com/exider_company\">\r\n                        <img src=\"https://lh3.google.com/u/0/d/1XMjb8E4Kgw--4yLgg4X-b4m4x2XtBlZg=w2000-h2408-iv1\" style=\"margin-right: 20px;\" class=\"app\">\r\n                    </a>\r\n                    <a href=\"https://twitter.com/exider_company\">\r\n                        <img src=\"https://lh3.google.com/u/0/d/1JMbJYPGBVWsXrAlogCRwte8K352HwwsS=w2000-h2408-iv1\" class=\"app\">\r\n                    </a>\r\n                </div>\r\n            </div>\r\n        </footer>\r\n    </div>\r\n</body>\r\n</html>";

        private readonly IValidationService _validationService;

        public EmailService(IValidationService validationService)
        {
            _validationService = validationService;
        }

        private async Task SendEmailAsync(string email, string template)
        {

            if (_validationService.ValidateEmail(email) == false)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentException(nameof(template));
            }

            MailAddress sender = new MailAddress(Configuration.corporateEmail, "Exider");
            MailAddress recipient = new MailAddress(email);
            MailMessage mail = new MailMessage(sender, recipient);

            mail.Subject = "Please confirm your email address";
            mail.Body = template;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential(Configuration.corporateEmail, Configuration.corporatePassword);
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);

        }

        public async Task SendEmailConfirmation(string email, 
            string code)
        {

            string template = _emailConfirmationHtml;

            template = template.Replace("CODE", "FFFFFF");

            await SendEmailAsync(email, template);

        }

        public async Task SendPasswordResetEmail(string email, string link, SessionModel model)
        {
            throw new NotImplementedException();
        }

        public async Task SendLoginNotificationEmail(string email, string link, SessionModel model)
        {
            throw new NotImplementedException();
        }

    }

}
