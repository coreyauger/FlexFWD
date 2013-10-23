using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Affine.Email
{
    public interface IEmailService
    {
        bool Send(string to, string subject, string body, string theme = null);
    }
}
