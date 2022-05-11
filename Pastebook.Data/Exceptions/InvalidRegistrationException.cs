using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Exceptions
{
    public class InvalidRegistrationException : Exception
    {
        public InvalidRegistrationException() : this("Invalid Registration Information")
        {

        }
        public InvalidRegistrationException(string message) : base(message)
        {

        }
    }
}
