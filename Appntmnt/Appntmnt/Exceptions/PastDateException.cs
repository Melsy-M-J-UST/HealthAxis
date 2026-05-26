using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class PastDateException : Exception
    {
        public PastDateException(string message) : base(message)
        {


        }
    }
}
