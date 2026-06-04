using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis_MVC.Exceptions
{
    public class AppointmentConflictException : Exception
    {
        public AppointmentConflictException(string message) : base(message)
        {

        }
    }
}
