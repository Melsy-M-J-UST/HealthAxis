using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class InvalidHealthrecordException : Exception
    {
        public InvalidHealthrecordException(string message) : base(message)
        {
        }
    }
}
