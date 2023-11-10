﻿using LibraryManagement.API.Services.Interface;
using System.Net.Mail;
using System.Net;

namespace LibraryManagement.API.Services.Implimentation
{
    public class EmailMessageService : IEmailMessageService
    {
        public async Task SendMessage(string email, string subject, string bodyHtml)
        {
            string fromAddress = "vedantp9@gmail.com";
            string toAddress = email;
            string password = "wqgn qsvm wzar mvsr";

            string contactInfoHtml = $@" 
                    <div style='max-width: 400px; margin: 0 left;'>
                        <p style='font-size: 16px; color: #333; font-weight: bold; text-align: center;'>Contact Information:</p>
                        <table style='font-size: 14px; color: #666; border-collapse: collapse; width: 100%; border: 1px solid #333; border-radius: 8px;'>
                            <tr>
                                <td style='padding: 8px;'>Name:</td>
                                <td style='padding: 8px;'>Vedant Patel</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px;'>Email:</td>
                                <td style='padding: 8px;'>vedantp9@gmail.com</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px;'>Phone:</td>
                                <td style='padding: 8px;'>+1 (647) 627 4235</td>
                            </tr>
                            <tr>
                                <td style='padding: 8px;'>LinkedIn:</td>
                                <td style='padding: 8px;'><a href=""https://www.linkedin.com/in/vedant-patel-38b743110/"" style='color: #0070f3; text-decoration: none;'>Vedant Patel on LinkedIn</a></td>
                            </tr>
                        </table>
                    </div>
                ";



            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromAddress, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = bodyHtml + contactInfoHtml,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toAddress);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}