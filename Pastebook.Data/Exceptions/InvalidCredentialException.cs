using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pastebook.Data.Exceptions
{
    public class InvalidCredentialException: Exception
    {
        public InvalidCredentialException() : this("Invalid Credential")
        {

        }
        public InvalidCredentialException(string message): base(message)
        {

        }
    }
}
