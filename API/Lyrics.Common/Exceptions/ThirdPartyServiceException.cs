using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Exceptions
{
    public class ThirdPartyServiceException:Exception
    {
        public ThirdPartyServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
