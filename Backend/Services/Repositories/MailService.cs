﻿using Backend.Models.Mail;
using Backend.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services.Repositories
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            string FilePath = "./Template/Mail/thanks.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            BoughtProducts htmlProduct = new BoughtProducts();

            foreach (var item in mailRequest.buyedProductsList)
            {
                htmlProduct.ProductName += $"<tr><td class={"tabel-style"}>{item.ProductName}</td><td class={"tabel-style"}>{item.Amount}</td><td class={"tabel-style"}>{item.TotalPrice}:-</td></tr>";
            }

            MailText = MailText.Replace("[orderid]", mailRequest.OrderId.ToString())
                               .Replace("[productname]", htmlProduct.ProductName)
                               .Replace("[username]", mailRequest.UserName)
                               .Replace("[address]", mailRequest.Address)
                               .Replace("[city]", mailRequest.City)
                               .Replace("[zipcode]", mailRequest.ZipCode)
                               .Replace("[orderdate]", mailRequest.Date)
                               .Replace("[discountname]", mailRequest.DiscountName)
                               .Replace("[discount]", mailRequest.Discount)
                               .Replace("[totalpricewithdiscount]", mailRequest.TotalPiceWithDiscount.ToString("0.##"));

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            email.Body = builder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                try
                {
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                    smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message);
                }
            }
        }
    }
}