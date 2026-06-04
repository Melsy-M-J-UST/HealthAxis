using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis_MVC.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException() 
        { 
        
        }
        public DoctorNotFoundException(string message) : base(message)
        {
        }
    }
}
