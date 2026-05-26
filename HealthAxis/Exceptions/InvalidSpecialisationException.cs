using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    public class InvalidSpecialisationException : Exception
    {
        public InvalidSpecialisationException(string message) : base(message){ }
    }
}
