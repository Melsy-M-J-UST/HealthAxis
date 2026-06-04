using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message) { }
    }
}