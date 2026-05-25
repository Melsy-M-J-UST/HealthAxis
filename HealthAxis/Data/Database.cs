using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Data
{
    public class Database
    {
        public List<Patient> Patients { get; set; } = new();
        public List<Doctor> Doctors { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public List<HealthRecord> HealthRecords { get; set; } = new();

        private int _nextPatientId = 1;
        private int _nextDoctorId = 1;
        private int _nextAppointmentId = 1;
        private int _nextHealthRecordId = 1;
        private int _nextSlotId = 1;

        public List<string> DailySlots { get; set; } = new()
        {
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "02:00 PM",
            "03:00 PM"
        };

        public Database()
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

        public int GetNextSlotId()
        {
            return _nextSlotId++;
        }
        public void Reset()
        {
            Patients.Clear();
            Doctors.Clear();
            Appointments.Clear();
            HealthRecords.Clear();

            _nextPatientId = 1;
            _nextDoctorId = 1;
            _nextAppointmentId = 1;
            _nextHealthRecordId = 1;

            SeedData();
        }

        private void SeedData()
        {
            Patients.AddRange(new List<Patient>
            {
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    PatientName = "Arun Kumar",
                    DateOfBirth = new DateTime(1992, 5, 14, 12, 24, 33),
                    Gender = Patient.Genders.Male,
                    PhoneNumber = "9876543210",
                    Email = "arun.kumar@example.com",
                    InsuranceId = "INS1001",
                    RegisteredDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    PatientName = "Meera Nair",
                    DateOfBirth = new DateTime(1988, 9, 22, 22, 15, 30),
                    Gender = Patient.Genders.Male,
                    PhoneNumber = "9876543211",
                    Email = "meera.nair@example.com",
                    InsuranceId = "INS1002",
                    RegisteredDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    PatientName = "Rahul Menon",
                    DateOfBirth = new DateTime(2000, 1, 10, 16, 17, 18),
                    Gender = Patient.Genders.Male,
                    PhoneNumber = "9876543212",
                    Email = "rahul.menon@example.com",
                    InsuranceId = "INS1003",
                    RegisteredDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    PatientName = "Anjali Thomas",
                    DateOfBirth = new DateTime(1995, 12, 3, 01, 02, 03),
                    Gender = Patient.Genders.Male,
                    PhoneNumber = "9876543213",
                    Email = "anjali.thomas@example.com",
                    InsuranceId = "INS1004",
                    RegisteredDate = DateTime.Now
                },
                new Patient
                {
                    PatientId = GetNextPatientId(),
                    PatientName = "Vivek Pillai",
                    DateOfBirth = new DateTime(1983, 7, 19, 5,6,7),
                    Gender = Patient.Genders.Male,
                    PhoneNumber = "9876543214",
                    Email = "vivek.pillai@example.com",
                    InsuranceId = "INS1005",
                    RegisteredDate = DateTime.Now
                }
            });

            Doctors.AddRange(new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Priya Sharma",
                    Specialisation = Doctor.Specialisations.Cardiologist,
                    Experience = 12,
                    Fees = 800,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Suresh Mathew",
                    Specialisation = Doctor.Specialisations.Dermatologist,
                    Experience = 9,
                    Fees = 600,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Neha Iyer",
                    Specialisation = Doctor.Specialisations.Pediatrician,
                    Experience = 10,
                    Fees = 700,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Thomas George",
                    Specialisation = Doctor.Specialisations.OrthopedicSurgeon,
                    Experience = 15,
                    Fees = 900,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Kavitha Rao",
                    Specialisation = Doctor.Specialisations.Neurologist,
                    Experience = 14,
                    Fees = 1000,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Mohammed Ali",
                    Specialisation = Doctor.Specialisations.GeneralPractitioner,
                    Experience = 11,
                    Fees = 500,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Lakshmi Menon",
                    Specialisation = Doctor.Specialisations.Endocrinologist,
                    Experience = 8,
                    Fees = 550,
                    IsPractising = true
                },
                new Doctor
                {
                    DoctorId = GetNextDoctorId(),
                    DoctorName = "Dr. Rajesh Nambiar",
                    Specialisation = Doctor.Specialisations.Oncologist,
                    Experience = 13,
                    Fees = 650,
                    IsPractising = true
                }
            });
        }
    }
}
