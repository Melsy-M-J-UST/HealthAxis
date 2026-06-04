using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis_MVC.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}
