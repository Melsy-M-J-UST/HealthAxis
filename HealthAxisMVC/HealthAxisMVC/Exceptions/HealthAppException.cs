using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Exceptions
{
    public class HealthAppException: Exception
    {
        public HealthAppException()
        {
            
        }
        public HealthAppException(string message) : base(message)
        {

        }
    }
}