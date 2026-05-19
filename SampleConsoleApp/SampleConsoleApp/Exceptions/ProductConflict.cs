using System;
using System.Collections.Generic;
using System.Text;

namespace SampleConsoleApp.Exceptions
{
    public class ProductConflict : Exception
    {
        public ProductConflict(string message):base(message)
        {
            
        }
    }
}
