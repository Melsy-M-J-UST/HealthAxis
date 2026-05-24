using HealthAxis.Models;

namespace HealthAxis.Data
{
    public class AppDbContext
    {
        public List<Patient> Patients { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public List<HealthRecord> HealthRecords { get; set; } = new();

        public List<string> DailySlots { get; set; } = new()
        {
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "02:00 PM",
            "03:00 PM"
        };

        private int _nextPatientId = 1;
        private int _nextDoctorId = 1;
        private int _nextAppointmentId = 1;
        private int _nextHealthRecordId = 1;

        public AppDbContext()
        {
            SeedData();
        }

        public int GetNextPatientId()
        {
            return _nextPatientId++;
        }

        public int GetNextDoctorId()
        {
            return _nextDoctorId++;
        }

        public int GetNextAppointmentId()
        {
            return _nextAppointmentId++;
        }

        public int GetNextHealthRecordId()
        {
            return _nextHealthRecordId++;
        }

        private void SeedData()
        {
            Patients.AddRange(new List<Patient>
            {
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    FullName = "Arun Kumar",
                    DateOfBirth = new DateTime(1992, 5, 14),
                    Gender = Patient.GenderOptions.Male,
                    PhoneNumber = "9876543210",
                    Email = "arun.kumar@example.com",
                    InsuranceId = "INS1001",
                    CreatedDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    FullName = "Meera Nair",
                    DateOfBirth = new DateTime(1988, 9, 22),
                    Gender = Patient.GenderOptions.Female,
                    PhoneNumber = "9876543211",
                    Email = "meera.nair@example.com",
                    InsuranceId = "INS1002",
                    CreatedDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    FullName = "Rahul Menon",
                    DateOfBirth = new DateTime(2000, 1, 10),
                    Gender = Patient.GenderOptions.Male,
                    PhoneNumber = "9876543212",
                    Email = "rahul.menon@example.com",
                    InsuranceId = "INS1003",
                    CreatedDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    FullName = "Anjali Thomas",
                    DateOfBirth = new DateTime(1995, 12, 3),
                    Gender = Patient.GenderOptions.Female,
                    PhoneNumber = "9876543213",
                    Email = "anjali.thomas@example.com",
                    InsuranceId = "INS1004",
                    CreatedDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    FullName = "Vivek Pillai",
                    DateOfBirth = new DateTime(1983, 7, 19),
                    Gender = Patient.GenderOptions.Male,
                    PhoneNumber = "9876543214",
                    Email = "vivek.pillai@example.com",
                    InsuranceId = "INS1005",
                    CreatedDate = DateTime.Now
                }
            });

            Doctors.AddRange(new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Priya Sharma",
                    Specialisation = Doctor.SpecialisationOption.Cardiologist,
                    YearsOfExperience = 12,
                    ConsultationFee = 800,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Suresh Mathew",
                    Specialisation = Doctor.SpecialisationOption.Dermatologist,
                    YearsOfExperience = 9,
                    ConsultationFee = 600,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Neha Iyer",
                    Specialisation = Doctor.SpecialisationOption.Pediatrician,
                    YearsOfExperience = 10,
                    ConsultationFee = 700,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Thomas George",
                    Specialisation = Doctor.SpecialisationOption.OrthopedicSurgeon,
                    YearsOfExperience = 15,
                    ConsultationFee = 900,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Kavitha Rao",
                    Specialisation = Doctor.SpecialisationOption.Neurologist,
                    YearsOfExperience = 14,
                    ConsultationFee = 1000,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Mohammed Ali",
                    Specialisation = Doctor.SpecialisationOption.GeneralPractitioner,
                    YearsOfExperience = 11,
                    ConsultationFee = 500,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Lakshmi Menon",
                    Specialisation = Doctor.SpecialisationOption.Oncologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 550,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Anita Joseph",
                    Specialisation = Doctor.SpecialisationOption.Gynecologist,
                    YearsOfExperience = 7,
                    ConsultationFee = 600,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Vikram Patel",
                    Specialisation = Doctor.SpecialisationOption.Psychiatrist,
                    YearsOfExperience = 10,
                    ConsultationFee = 700,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    FullName = "Rajesh Nambiar",
                    Specialisation = Doctor.SpecialisationOption.Endocrinologist,
                    YearsOfExperience = 13,
                    ConsultationFee = 650,
                    IsActive = true
                }
            });
        }
    }
}