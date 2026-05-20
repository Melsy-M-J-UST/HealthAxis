using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    public class AppointmentNotFoundException :Exception
    {
        public AppointmentNotFoundException(string message) : base(message) { }
    }
}
