using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Data
{
    public class Database
    {
        public List<Patient> Patients { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<HealthRecord> HealthRecords { get; set; }

        private int _nextPatientId = 1;
        private int _nextDoctorId = 1;
        private int _nextAppointmentId = 1;
        private int _nextHealthRecordId = 1;
        private int _nextSlotId = 1;

        public List<string> DailySlots { get; set; }= new List<string>
        {
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "02:00 PM",
            "03:00 PM"
        };

        public Database()
        {
            Patients = new List<Patient>();
            Doctors = new List<Doctor>();
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

        private Patient CreatePatient(string name, DateTime dob, Patient.Genders gender, string phone, string email, string insuranceId)
        {
            return new Patient
            {
                PatientId = GetNextPatientId(),
                PatientName = name,
                DateOfBirth = dob,
                Gender = gender,
                PhoneNumber = phone,
                Email = email,
                InsuranceId = insuranceId,
                RegisteredDate = DateTime.Now
            };
        }
        private Doctor CreateDoctor(string name, Doctor.Specialisations specialization, int experience, int fees)
        {
            return new Doctor
            {
                DoctorId = GetNextDoctorId(),
                DoctorName = name,
                Specialisation = specialization,
                Experience = experience,
                Fees = fees,
                IsActive = true
            };
        }
        private void SeedData()
        {
            Patients.AddRange(new List<Patient>
            {
                CreatePatient("Arun Kumar",new DateTime(1992, 5, 14, 12, 24, 33, DateTimeKind.Unspecified),Patient.Genders.Male,"9876543210","arun.kumar@example.com","INS1001"),
                CreatePatient("Meera Nair",new DateTime(1988, 9, 22, 22, 15, 30, DateTimeKind.Unspecified), Patient.Genders.Female, "9876543211","meera.nair@example.com","INS1002"),
                CreatePatient("Rahul Menon", new DateTime(2000, 1, 10, 16, 17, 18, DateTimeKind.Unspecified),Patient.Genders.Male, "9876543212", "rahul.menon@example.com","INS1003"),
                CreatePatient("Anjali Thomas", new DateTime(1995, 12, 3, 01, 02, 03, DateTimeKind.Unspecified),Patient.Genders.Female,"9876543213","anjali.thomas@example.com","INS1004"),
                CreatePatient("Vivek Pillai",new DateTime(1983, 7, 19, 5, 6, 7, DateTimeKind.Unspecified),Patient.Genders.Male,"9876543214","vivek.pillai@example.com","INS1005")
            });
            Doctors.AddRange(new List<Doctor>
            {
                CreateDoctor("Dr. Priya Sharma", Doctor.Specialisations.Cardiologist, 12, 800),
                CreateDoctor("Dr. Suresh Mathew", Doctor.Specialisations.Dermatologist, 9, 600),
                CreateDoctor("Dr. Neha Iyer", Doctor.Specialisations.Pediatrician, 10, 700),
                CreateDoctor("Dr. Thomas George", Doctor.Specialisations.OrthopedicSurgeon, 15, 900),
                CreateDoctor("Dr. Kavitha Rao", Doctor.Specialisations.Neurologist, 14, 1000),
                CreateDoctor("Dr. Mohammed Ali", Doctor.Specialisations.GeneralPractitioner, 11, 500),
                CreateDoctor("Dr. Lakshmi Menon", Doctor.Specialisations.Endocrinologist, 8, 550),
                CreateDoctor("Dr. Rajesh Nambiar", Doctor.Specialisations.Oncologist, 13, 650)
            });
        }
    }
}