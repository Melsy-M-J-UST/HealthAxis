using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Exceptions
{
    public class HealthrecordNotFoundException : Exception
    {
        public HealthrecordNotFoundException(string message) : base(message)
        {
        }
    }
}
