using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Exceptions
{
    public class InvalidHealthRecordException : Exception
    {
        public InvalidHealthRecordException(string Message) : base(Message)
        {
        }
    }
}