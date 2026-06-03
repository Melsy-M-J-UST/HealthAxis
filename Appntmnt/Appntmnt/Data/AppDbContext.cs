using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Data
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
            AddData();
        }

        public int GetNextPatientId() => _nextPatientId++;

        public int GetNextDoctorId() => _nextDoctorId++;

        public int GetNextAppointmentId() => _nextAppointmentId++;

        public int GetNextHealthRecordId() => _nextHealthRecordId++;

        private void AddData()
        {
            var patientAddData = new[]
            {
                ("Arun Kumar", new DateTime(1992, 5, 14, 0, 0, 0, DateTimeKind.Local), Patient.GenderOptions.Male, "9876543210", "arun.kumar@example.com", "INS1001"),
                ("Meera Nair", new DateTime(1988, 9, 22, 0, 0, 0, DateTimeKind.Local), Patient.GenderOptions.Female, "9876543211", "meera.nair@example.com", "INS1002"),
                ("Rahul Menon", new DateTime(2000, 1, 10, 0, 0, 0, DateTimeKind.Local), Patient.GenderOptions.Male, "9876543212", "rahul.menon@example.com", "INS1003"),
                ("Anjali Thomas", new DateTime(1995, 12, 3, 0, 0, 0, DateTimeKind.Local), Patient.GenderOptions.Female, "9876543213", "anjali.thomas@example.com", "INS1004"),
                ("Vivek Pillai", new DateTime(1983, 7, 19, 0, 0, 0, DateTimeKind.Local), Patient.GenderOptions.Male, "9876543214", "vivek.pillai@example.com", "INS1005")
            };

            Patients.AddRange(
                patientAddData.Select(p => CreatePatient(
                    p.Item1,
                    p.Item2,
                    p.Item3,
                    p.Item4,
                    p.Item5,
                    p.Item6))
            );

            var doctorAddData = new[]
            {
                ("Priya Sharma", Doctor.SpecialisationOption.Cardiologist, 12, 800, true),
                ("Suresh Mathew", Doctor.SpecialisationOption.Dermatologist, 9, 600, true),
                ("Neha Iyer", Doctor.SpecialisationOption.Pediatrician, 10, 700, true),
                ("Thomas George", Doctor.SpecialisationOption.OrthopedicSurgeon, 15, 900, true),
                ("Kavitha Rao", Doctor.SpecialisationOption.Neurologist, 14, 1000, true),
                ("Mohammed Ali", Doctor.SpecialisationOption.GeneralPractitioner, 11, 500, true),
                ("Lakshmi Menon", Doctor.SpecialisationOption.Oncologist, 8, 550, true),
                ("Anita Joseph", Doctor.SpecialisationOption.Gynecologist, 7, 600, true),
                ("Vikram Patel", Doctor.SpecialisationOption.Psychiatrist, 10, 700, true),
                ("Rajesh Nambiar", Doctor.SpecialisationOption.Endocrinologist, 13, 650, true)
            };

            Doctors.AddRange(
                doctorAddData.Select(d => CreateDoctor(
                    d.Item1,
                    d.Item2,
                    d.Item3,
                    d.Item4,
                    d.Item5))
            );
        }

        private Patient CreatePatient(
            string fullName,
            DateTime dateOfBirth,
            Patient.GenderOptions gender,
            string phoneNumber,
            string email,
            string insuranceId)
        {
            return new Patient
            {
                PatientId = GetNextPatientId(),
                FullName = fullName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                PhoneNumber = phoneNumber,
                Email = email,
                InsuranceId = insuranceId,
                CreatedDate = DateTime.Now
            };
        }

        private Doctor CreateDoctor(
            string fullName,
            Doctor.SpecialisationOption specialisation,
            int yearsOfExperience,
            int consultationFee,
            bool isActive)
        {
            return new Doctor
            {
                DoctorId = GetNextDoctorId(),
                FullName = fullName,
                Specialisation = specialisation,
                YearsOfExperience = yearsOfExperience,
                ConsultationFee = consultationFee,
                IsActive = isActive
            };
        }
    }
}
