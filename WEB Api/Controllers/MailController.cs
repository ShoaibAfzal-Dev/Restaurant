using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Macs;
using System.Net;
using System.Net.Mail;
using WEB_Api.Migrations;
using WEB_Api.Models;

namespace WEB_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MyDbContext Db;
        public MailController(IConfiguration configuration, MyDbContext _Db)
        {
            _configuration = configuration;
            Db = _Db;
        }

        [HttpPost]
        [Route("Emailsending")]
        public async Task<IActionResult> SendEmail(int id)
        {
            var orderData = Db.Order_Details
                .Where(order => order.Status == true && order.Id == id)
                .Select(order => new
                {
                    order.Id,
                    order.item,
                    order.Price,
                    order.quantity,
                    order.Instructions,
                    sub_details = Db.Order_SubDetails
                        .Where(sub_order => sub_order.Order_DetailsId == order.Id)
                        .Select(sub_order => new
                        {
                            sub_order.Id,
                            sub_order.details,
                            sub_order.price,
                            sub_order.Order_DetailsId,
                        })
                        .ToList(),
                    user_details = Db.User_details
                        .Where(user => user.Id == order.User_detailsId)
                        .Select(user => new
                        {
                            user.Id,
                            user.FirstName,
                            user.LastName,
                            user.Email,
                            user.Phone,
                            user.Day,
                            user.Card,
                            user.Cash
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (orderData == null)
            {
                return NotFound();
            }

            
            string htmlBody = BuildHtmlBody(orderData);

            
            await SendEmail(orderData.user_details.Email, "Order Details", htmlBody);

            var find = Db.Order_Details.FirstOrDefault(s => s.Id == id);
            if (find != null)
            {
                find.OrderStatus = true;
                Db.Order_Details.Update(find);
                Db.SaveChanges();
            }


            return Ok(true);
        }

        private string BuildHtmlBody(dynamic orderData)
        {
            
            string htmlBody = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <style>
                {GetCssStyles()}
            </style>
        </head>
        <body>
            {GetOrderDetailsHtml(orderData)}
        </body>
        </html>";

            return htmlBody;
        }

        private string GetCssStyles()
        {
            
            string cssStyles = @"
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            padding: 20px;
        }
        .order-container {
            background-color: #fff;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin-bottom: 20px;
        }
        .sub-details-container {
            margin-top: 10px;
        }
        .user-details {
            margin-top: 20px;
        }
        h2 {
            color: #333;
        }
        h3 {
            color: #555;
        }
        p {
            color: #777;
        }";

            return cssStyles;
        }

        private string GetOrderDetailsHtml(dynamic orderData)
        {
            
            string orderDetailsHtml = $@"
        <div class='order-container'>
            <h2>Order Details</h2>
            <p><strong>Order ID:</strong> #{orderData.Id}</p>
            <p><strong>Item:</strong> {orderData.item}</p>
            <p><strong>Price:</strong> ${orderData.Price}</p>
            <p><strong>Quantity:</strong> {orderData.quantity}</p>
            <p><strong>Instructions:</strong> {orderData.Instructions}</p>";

           
            if (orderData.sub_details.Count>0)
            {
                orderDetailsHtml += @"
            <div class='sub-details-container'>
                <h3>Sub Details</h3>
                <ul>";

                foreach (var subDetail in orderData.sub_details)
                {
                    orderDetailsHtml += $@"
                    <li><strong>Details:</strong> {subDetail.details}</li>
                    <li><strong>Price:</strong> ${subDetail.price}</li>";
                }

                orderDetailsHtml += @"
                </ul>
            </div>";
            }

            
            orderDetailsHtml += $@"
            <div class='user-details'>
                <h3>User Details</h3>
                <p><strong>Name:</strong> {orderData.user_details.FirstName} {orderData.user_details.LastName}</p>
                <p><strong>Email:</strong> {orderData.user_details.Email}</p>
                <p><strong>Phone:</strong> {orderData.user_details.Phone}</p>
                <p><strong>Day:</strong> {orderData.user_details.Day}</p>

                    </div>
                </div> ";
        
    return orderDetailsHtml;
        }
        private async Task SendEmail(string toEmail, string subject, string body)
        {
           
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(""),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(new MailAddress(""));

                using var client = new SmtpClient("")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(""),
                EnableSsl = true
                };

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception)
            {
                
                throw;
            }
        }


    }
}
