using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message)
        {
        }
    }
}
