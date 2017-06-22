using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesLibrary
{
    public class ItemAllreadyExistsException : Exception
    {
        public ItemAllreadyExistsException(string message) : base(message)
        {
        }
    }

    public class EmptyFieldException : Exception
    {
        public EmptyFieldException(string message) : base(message)
        {
        }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message)
        {
        }
    }
}
