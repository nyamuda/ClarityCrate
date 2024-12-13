using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;


namespace Clarity_Crate.Services
{
    public class EmailSender : IEmailSender
    {


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var messageToSend = new MimeMessage();
                messageToSend.From.Add(new MailboxAddress("Clarity Crate", "cratecrarity@gmail.com"));
                messageToSend.To.Add(new MailboxAddress(email, "ptnrlab@gmail.com"));
                messageToSend.Subject = subject;



                //send the body as HTML
                messageToSend.Body = new TextPart("html")
                {
                    Text = message
                };


                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("crateclarity@gmail.com", "ltqi asfs apgj nuax");

                    client.Send(messageToSend);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            
            }
        }


        /*
      public async Task SendEmailAsync(string email, string subject, string message)
      {
          try
          {
              var mimeMessage = new MimeMessage();
              mimeMessage.From.Add(new MailboxAddress("Clarity Crate", "nya20002@byui.edu")); // Use your verified email
              mimeMessage.To.Add(new MailboxAddress("Recipient", email));  // User's email
              mimeMessage.Subject = subject;

              mimeMessage.Body = new TextPart("html")
              {
                  Text = message
              };

              using (var client = new SmtpClient())
              {
                  // Connect to SMTP2GO's SMTP server
                  await client.ConnectAsync("mail.smtp2go.com", 2525, false);

                  // Authenticate with your username and password
                  await client.AuthenticateAsync("byui.edu", "9uUGRpC4muWKRpUK");

                  // Send the email
                  await client.SendAsync(mimeMessage);

                  // Disconnect after sending
                  await client.DisconnectAsync(true);

                  Console.WriteLine("Finished sending");
              }


          }
          catch (Exception ex)
          {

              Console.WriteLine(ex);
          }
      }

      */




    }
}
