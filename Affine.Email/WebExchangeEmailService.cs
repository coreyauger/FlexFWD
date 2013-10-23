using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;

namespace Affine.Email
{
    public class WebExchangeEmailService : IEmailService
    {
        private static ExchangeService _service = GetBinding();

        static ExchangeService GetBinding()
        {
            // Create the binding.
            ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);

            // Define credentials.
            service.Credentials = new WebCredentials(System.Configuration.ConfigurationManager.AppSettings["Affine.Email.User"], System.Configuration.ConfigurationManager.AppSettings["Affine.Email.Password"]);

            // Use the AutodiscoverUrl method to locate the service endpoint.
            try
            {
                service.AutodiscoverUrl(System.Configuration.ConfigurationManager.AppSettings["Affine.Email.User"], RedirectionUrlValidationCallback);
            }
            catch (AutodiscoverRemoteException ex)
            {
                Console.WriteLine("Exception thrown: " + ex.Error.Message);
            }
            return service;
        }

        static bool RedirectionUrlValidationCallback(String redirectionUrl)
        {
            // Perform validation.
            // Validation is developer dependent to ensure a safe redirect.
            Console.WriteLine(redirectionUrl);
            return true;
        }


        public WebExchangeEmailService()
        {
          
        }

        public bool Send(string to, string subject, string body, string theme = null)
        {
            EmailMessage message = new EmailMessage(_service);
            message.ToRecipients.Add(new EmailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.From = System.Configuration.ConfigurationManager.AppSettings["Affine.Email.User"];
            message.Send();
            return true;
        }
    }
}
