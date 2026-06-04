using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis_MVC.Exceptions
{
    public class PastDateException : Exception
    {
        public PastDateException(string message) : base(message)
        {
        }
    }
}
