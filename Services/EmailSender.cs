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
                messageToSend.From.Add(new MailboxAddress("Clarity Crate", "ptnrlab@gmail.com"));
                messageToSend.To.Add(new MailboxAddress(email, "ptnmath@gmail.com"));
                messageToSend.Subject = subject;



                //send the body as HTML
                messageToSend.Body = new TextPart("html")
                {
                    Text = message
                };
                /*
                messageToSend.Body = new TextPart("plain")
                {
                    Text = "Hey there my friend"
                };*/

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate("ptnrlab@gmail.com", "vghw owdn uncx lnoq");

                    client.Send(messageToSend);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            { }
        }


        //function will return HTML Code
        /*
        private string GetEmailTemplate()
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "ConfirmationEmail.html");
            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }


            return body;
        }
        */


    }



}
