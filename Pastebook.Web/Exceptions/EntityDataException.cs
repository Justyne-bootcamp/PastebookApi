using System;

namespace Pastebook.Web.Exceptions
{
    public class EntityDataException : Exception
    {
        public EntityDataException()
        {
        }

        public EntityDataException(string message)
            : base(message)
        {
        }

        public EntityDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
