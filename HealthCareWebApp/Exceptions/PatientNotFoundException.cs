using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
        }
    }
}