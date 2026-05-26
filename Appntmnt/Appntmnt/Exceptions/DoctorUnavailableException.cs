using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class DoctorUnavailableException : Exception
    {
        public DoctorUnavailableException(string message) : base(message)
        {
        }
    }
}
