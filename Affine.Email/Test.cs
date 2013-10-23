using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affine.Email
{
    public class Test
    {
        static void Main()
        {
            IEmailService service = EmailServiceFactory.CreateService("ExchangeOnline");
            service.Send("coreyauger@gmail.com", "test", "testing the service yo");

            Console.ReadKey();
        }
    }
}
