using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    internal class HealthRecordNotFoundException : Exception
    {
        public HealthRecordNotFoundException(string Message) : base(Message)
        {
        }
    }
}
