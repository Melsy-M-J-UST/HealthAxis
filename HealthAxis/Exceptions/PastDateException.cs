using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    public class PastDateException : Exception
    {
        public PastDateException(string message) : base(message)
        {


        }
    }
}
