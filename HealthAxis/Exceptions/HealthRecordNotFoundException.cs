using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    public class HealthRecordNotFoundException : Exception
    {
        public HealthRecordNotFoundException(string Message) : base(Message)
        {
        }
    }
}
