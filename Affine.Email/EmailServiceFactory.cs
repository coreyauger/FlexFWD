using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affine.Email
{
    public class EmailServiceFactory
    {
        public static IEmailService CreateService(string service)
        {
            switch (service)
            {
                case "ExchangeOnline":
                    //return new WebExchangeEmailService();d
                case "GMail":
                    return new GMailService();
            }
            return null;
        }
    }
}
