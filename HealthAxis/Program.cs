using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services;
using HealthAxis.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Linq;

var services = new ServiceCollection();
services.AddSingleton<Database>();
services.AddScoped<IPatientRepository, PatientRepository>();
services.AddScoped<IDoctorRepository, DoctorRepository>();
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IHealthRecordRepository, HealthRecordRepository>();


services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IDoctorService, DoctorService>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IHealthRecordService, HealthRecordService>();

var provider = services.BuildServiceProvider();

var db = provider.GetRequiredService<Database>();
IPatientService patientService = provider.GetRequiredService<IPatientService>();
IDoctorService doctorService = provider.GetRequiredService<IDoctorService>();
IAppointmentService appointmentService = provider.GetRequiredService<IAppointmentService>();
IHealthRecordService healthRecordService = provider.GetRequiredService<IHealthRecordService>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Appointment Portal =====");
    Console.WriteLine("1. Register a new patient");
    Console.WriteLine("2. Add a new doctor");
    Console.WriteLine("3. Search doctors by specialisation");
    Console.WriteLine("4. Book an appointment for a patient");
    Console.WriteLine("5. View all appointments for a patient");
    Console.WriteLine("6. Confirm, cancel, or complete an appointment");
    Console.WriteLine("7. Add a health record after a completed appointment");
    Console.WriteLine("8. View health history for a patient");
    Console.WriteLine("9. View all patients");
    Console.WriteLine("10. View all doctors");

    Console.WriteLine("11. View upcoming confirmed appointments");
    Console.WriteLine("12. Exit");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            RegisterPatient();
            break;
        case "2":
            AddDoctor();
            break;
        case "3":
            SearchDoctorsBySpecialisation();
            break;
        case "4":
            BookAppointment();
            break;
        case "5":
            //ViewAppointmentsForPatient();
            break;
        case "6":
            ConfirmCancelOrCompleteAppointment();
            break;
        case "7":
            AddHealthRecord();
            break;
        case "8":
            ViewHealthHistory();
            break;
        case "9":
            ViewAllPatients();
            break;
        case "10":
            ViewAllDoctors();
            break;
        case "11":
            //ViewUpcomingConfirmedAppointments();
            break;
        case "12":
            Console.WriteLine("Exiting application...");
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void RegisterPatient()
{
    Patient p = new Patient();
    p.PatientId = db.GetNextPatientId();

    Console.Write("Enter your full name: ");
    string fullName = Console.ReadLine() ?? string.Empty;

    if (!Regex.IsMatch(fullName, @"^[A-Za-z]+( [A-Za-z]+)*$"))
    {
        Console.WriteLine("Invalid name.");
        return;
    }
    p.FullName = fullName;

    Console.Write("Enter your Date of Birth (YYYY-MM-DD): ");
    string dobInput = Console.ReadLine();

    if (!DateTime.TryParse(dobInput, out DateTime dob) || dob > DateTime.Today)
    {
        Console.WriteLine("Invalid date of birth.");
        return;
    }
    p.DateOfBirth = dob;

    Console.Write("Enter gender (Male/Female/Transgender/Other): ");
    if (!Enum.TryParse(Console.ReadLine(), true, out Patient.GenderOptions gender))
    {
        Console.WriteLine("Invalid gender.");
        return;
    }
    p.Gender = gender;

    Console.Write("Enter phone number: ");
    string phone = Console.ReadLine() ?? string.Empty;

    if (!Regex.IsMatch(phone, @"^\d{10}$"))
    {
        Console.WriteLine("Invalid phone number.");
        return;
    }
    p.PhoneNumber = phone;

    Console.Write("Enter email: ");
    string email = Console.ReadLine() ?? string.Empty;

    if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        Console.WriteLine("Invalid email.");
        return;
    }
    p.Email = email;

    Console.Write("Enter Insurance ID: ");
    p.InsuranceID = Console.ReadLine() ?? string.Empty;

    p.CreatedDate = DateTime.Now;

    patientService.RegisterPatient(p);
    Console.WriteLine("Patient registered successfully");
}
void AddDoctor()
{
    try
    {
        Doctor doctor = new Doctor();

        doctor.DoctorId = db.GetNextDoctorId();

        Console.Write("Enter Full Name: ");
        doctor.FullName = Console.ReadLine();

        doctor.Specialisation = GetSpecialisationFromUser();

        Console.Write("Enter Years of Experience: ");
        doctor.YearsOfExperience = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Consultation Fee: ");
        doctor.ConsultationFee = Convert.ToInt32(Console.ReadLine());

        Console.Write("Is Active (true/false): ");
        doctor.IsActive = Convert.ToBoolean(Console.ReadLine());

        doctorService.AddDoctor(doctor);

        Console.WriteLine("Doctor added successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
void SearchDoctorsBySpecialisation()
{
    try
    {
        var specialization = GetSpecialisationFromUser();

        var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

        foreach (var doctor in doctors)
        {
            Console.WriteLine(doctor.GetProfileSummary());
        }
    }
    catch (DoctorNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
Doctor.SpecialisationOption GetSpecialisationFromUser()
{
    Console.WriteLine("Choose Specialisation:");

    var specialisations = Enum.GetValues(typeof(Doctor.SpecialisationOption));

    for (int i = 0; i < specialisations.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {specialisations.GetValue(i)}");
    }

    Console.Write("Enter choice (number): ");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        throw new Exception("Invalid input! Please enter a number.");
    }

    if (choice < 1 || choice > specialisations.Length)
    {
        throw new Exception("Choice out of range!");
    }

    return (Doctor.SpecialisationOption)specialisations.GetValue(choice - 1);
}

void BookAppointment()
{
    try
    {
        Console.Write("Enter Patient ID: ");
        int patientId = int.Parse(Console.ReadLine() ?? "0");

        var patient = patientService.GetPatientById(patientId);

        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        var specialization = GetSpecialisationFromUser();

        var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

        if (doctors == null || !doctors.Any())
        {
            Console.WriteLine("No doctors available for this specialization.");
            return;
        }

        Console.WriteLine("\nAvailable Doctors:");
        foreach (var d in doctors)
        {
            Console.WriteLine($"ID: {d.DoctorId}, Name: {d.FullName}, Exp: {d.YearsOfExperience} yrs, Fee: {d.ConsultationFee}");
        }

        Console.Write("\nEnter Doctor ID: ");
        int doctorId = int.Parse(Console.ReadLine() ?? "0");

        var doctor = doctors.FirstOrDefault(d => d.DoctorId == doctorId);

        if (doctor == null)
        {
            Console.WriteLine("Invalid doctor selection.");
            return;
        }

        Console.Write("Enter appointment date (yyyy-MM-dd): ");
        DateTime date = DateTime.Parse(Console.ReadLine() ?? string.Empty);

        Console.Write("Enter time slot (e.g., 10AM-11AM): ");
        string slot = Console.ReadLine() ?? string.Empty;

        var appointment = new Appointment
        {
            Patient = patient,
            Doctor = doctor,
            ScheduledDate = date,
            Slot = slot
        };

        var bookedAppointment = appointmentService.BookAppointment(appointment);

        Console.WriteLine("\nAppointment booked successfully!");
        Console.WriteLine(bookedAppointment.GetDetails());
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
}
void ViewAllPatients()
{
    var patients = patientService.GetAllPatients();
    if (!patients.Any())
    {
        Console.WriteLine("No Patients Found");
        return;
    }
    foreach (var patient in patients)
    {
        Console.WriteLine(patient.GetProfileSummary());
    }
}
void ViewAllDoctors()
{
    var doctors = doctorService.GetAllDoctors();
    if (!doctors.Any())
    {
        Console.WriteLine("No Doctors Found");
        return;
    }
    foreach (var doctor in doctors)
    {
        Console.WriteLine(doctor.GetProfileSummary());
    }
}

void ConfirmCancelOrCompleteAppointment()
{
    try
    {
        Console.Write("Enter your Appointment ID: ");
        int appointmentId = int.Parse(Console.ReadLine() ?? "0");

        var appointment = appointmentService.GetAppointmentById(appointmentId);

        if (appointment == null)
        {
            Console.WriteLine($"Appointment with Id {appointmentId} not found.");
            return;
        }

        Console.Write($"We have your Apponintmentwith Id {appointmentId}. Please choose the below option to make changes to the status of your Appointment.");
        Console.WriteLine("Press 1 to Confirm your appointment");
        Console.WriteLine("Press 2 to Cancel your appointment");
        Console.WriteLine("Press 3 to Complete your appointrment");

        string action = Console.ReadLine() ?? string.Empty;


        if (action == "1")
        {
            appointment.Confirm();
            Console.WriteLine("Appointment confirmed.");
        }
        else if (action == "2")
        {
            Console.Write("Cancellation reason: ");
            string reason = Console.ReadLine() ?? string.Empty;
            appointmentService.CancelAppointment(appointmentId, reason);
            Console.WriteLine("Appointment cancelled.");
        }
        else if (action == "3")
        {
            appointment.Status = Appointment.AppointmentStatus.Completed;
            Console.WriteLine("Appointment completed.");
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }

        Console.WriteLine(appointment);
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");

    }
}

void AddHealthRecord()
{
    Console.Write("Enter Appointment ID: ");

    if (!int.TryParse(Console.ReadLine(), out int appointmentId))
    {
        Console.WriteLine("Invalid Appointment ID.");
        return;
    }

    var appointment = appointmentService.GetAppointmentById(appointmentId);

    if (appointment == null)
    {
        Console.WriteLine("Appointment not found.");
        return;
    }

    if (appointment.Status != Appointment.AppointmentStatus.Completed)
    {
        Console.WriteLine("Health records can only be added for completed appointments.");
        return;
    }

    HealthRecord record = new HealthRecord();

    record.Patient = appointment.Patient;
    record.Doctor = appointment.Doctor;
    record.VisitDate = appointment.ScheduledDate;

    Console.Write("Enter Diagnosis: ");
    record.Diagnosis = Console.ReadLine() ?? string.Empty;

    Console.Write("Enter Prescription: ");
    record.Prescription = Console.ReadLine() ?? string.Empty;

    Console.Write("Enter Additional Notes: ");
    record.Notes = Console.ReadLine() ?? string.Empty;

    healthRecordService.AddRecord(record);

    Console.WriteLine("Health record added successfully.");
}

void ViewHealthHistory()
{
    Console.Write("Enter Patient ID: ");

    if (!int.TryParse(Console.ReadLine(), out int patientId))
    {
        Console.WriteLine("Invalid Patient ID.");
        return;
    }

    var records = healthRecordService.GetRecordsByPatient(patientId);

    if (records == null || !records.Any())
    {
        Console.WriteLine("No health records found for this patient.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Health History =====");

    foreach (var record in records)
    {
        Console.WriteLine(record.GetRecordSummary());
        Console.WriteLine("-----------------------------------");
    }
}