using Qct.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Repository.Exceptions
{
    public class NotFoundPayConfigurationException : QCTException
    {
        public NotFoundPayConfigurationException(string msg) : base(msg)
        {

        }
    }
}
