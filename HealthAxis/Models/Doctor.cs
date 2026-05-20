using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int DoctorName { get; set; }
        public int Specialization { get; set; }
        public int Experience { get; set; }
        public int Fees { get; set; }
        public bool IsPractising { get; set; }


    }
}
