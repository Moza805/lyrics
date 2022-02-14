using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Exceptions
{
    /// <summary>
    /// Thrown when a third party API does not respond as expected 
    /// E.g. 500 error
    /// </summary>
    public class ThirdPartyServiceException : Exception
    {
        public ThirdPartyServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
