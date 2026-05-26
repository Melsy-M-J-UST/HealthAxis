using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}
