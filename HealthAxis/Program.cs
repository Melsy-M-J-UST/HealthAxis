using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services;
using HealthAxis.Services.Impl;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Transactions;

var services = new ServiceCollection();
services.AddSingleton<AppDbContext>();
services.AddScoped<IPatientRepository, PatientRepository>();
services.AddScoped<IDoctorRepository, DoctorRepository>();
services.AddScoped<IAppointmentRepository, AppointmentRepository>();
services.AddScoped<IHealthRepository, HealthRepository>();


services.AddScoped<IPatientService, PatientService>();
services.AddScoped<IDoctorService, DoctorService>();
services.AddScoped<IAppointmentService, AppointmentService>();
services.AddScoped<IHealthRecordService, HealthRecordService>();

var provider = services.BuildServiceProvider();

var db = provider.GetRequiredService<AppDbContext>();
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
    Console.WriteLine("6.  Cancel, or Complete an appointment");
    Console.WriteLine("7. Add a health record after a completed appointment");
    Console.WriteLine("8. View health history for a patient");
    Console.WriteLine("9. View all patients");
    Console.WriteLine("10. View all doctors");
    Console.WriteLine("11. View upcoming confirmed appointments");
    Console.WriteLine("12. Make an Doctor Active/Inactive");
    Console.WriteLine("13. Exit");
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
            ViewAppointmentsForPatient();
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
            ViewUpcomingConfirmedAppointments();
            break;
        case "12":
            MakeDoctorActive();
            break;
        case "13":
            Console.WriteLine("Exiting application...");
            return;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
//1st function
void RegisterPatient()
{
    Patient p = new Patient();
    Console.Write("Enter your full name: ");
    string FullName = Console.ReadLine() ?? string.Empty;
    if (Regex.IsMatch(FullName, @"^[A-Za-z]+( [A-Za-z]+)*$"))
    {
        p.FullName = FullName;
    }
    else
    {
        Console.WriteLine("Enter a Valid Name");
        return;
    }

    Console.Write("Enter your Date of Birth(YYYY-MM-DD):\n");
    var DateOfBirth = Console.ReadLine();
    if (!DateTime.TryParse(DateOfBirth, out DateTime dateOfBirth) || dateOfBirth > DateTime.Today)
    {
        Console.WriteLine("Enter a Valid Date of Birth");
        return;
    }
    else
    {
        p.DateOfBirth = dateOfBirth;
    }

    Console.Write("Enter your Gender: \nMale\nFemale\nTransgender\nOther\nKindly please enter the one among the four given above.\n");
    bool G = Enum.TryParse(Console.ReadLine(), true, out Patient.GenderOptions gender);
    if (G)
    {
        p.Gender = gender;
    }
    else
    {
        Console.WriteLine("Enter Valid Gender From the list");
        return;
    }

    Console.Write("Enter your Phone number:");

    string PhoneNumber = Console.ReadLine() ?? string.Empty;
    if (Regex.IsMatch(PhoneNumber, @"^\d{10}$"))
    {
        p.PhoneNumber = PhoneNumber;
    }
    else
    {
        Console.WriteLine("Enter a valid Phone Number");
        return;
    }
    Console.Write("Enter your Mail Id: ");
    string Email = Console.ReadLine() ?? string.Empty;
    if (Email != string.Empty)
    {
        if (Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            p.Email = Email;
        }
        else
        {
            Console.WriteLine("Enter a Valid Email Id");
            return;
        }

    }
    Console.Write("Enter your Insurance ID: ");

    p.InsuranceId = Console.ReadLine() ?? string.Empty;
    DateTime now = DateTime.Now;
    p.CreatedDate = now;
    p.PatientId = db.GetNextPatientId();
    patientService.RegisterPatient(p);
    Console.WriteLine();
}
//2nd Function
void AddDoctor()
{
    try
    {
        Doctor doctor = new Doctor();

        Console.Write("Enter Full Name: ");
        string FullName = Console.ReadLine() ?? string.Empty;
        if (Regex.IsMatch(FullName, @"^[A-Za-z]+( [A-Za-z]+)*$"))
        {
            doctor.FullName = FullName;
        }
        else
        {
            Console.WriteLine("Enter a Valid name");
            return;
        }

        doctor.Specialisation = GetSpecialisationFromUser();

        Console.Write("Enter Years of Experience: ");
        doctor.YearsOfExperience = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Consultation Fee: ");
        doctor.ConsultationFee = Convert.ToInt32(Console.ReadLine());

        Console.Write("Is Active (true/false): ");
        doctor.IsActive = Convert.ToBoolean(Console.ReadLine());

        doctor.DoctorId = db.GetNextDoctorId();
        doctorService.AddDoctor(doctor);

        Console.WriteLine("Doctor added successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
//3rd Function
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

    Console.Write("Enter Specialisation Name ");
    string specialisationInput = Console.ReadLine();

    if (int.TryParse(specialisationInput, out int choice))
    {
        if (choice >= 1 && choice <= specialisations.Length)
        {
            return (Doctor.SpecialisationOption)specialisations.GetValue(choice - 1)!;
        }
    }

    if (Enum.TryParse(specialisationInput, true, out Doctor.SpecialisationOption result))
    {
        return result;
    }
    throw new Exception("Invalid Specialisation Entered");

}
//4th Function
void BookAppointment()
{
    try
    {
        Console.Write("Patient ID: ");
        int patientId = int.Parse(Console.ReadLine() ?? "0");

        var patient = patientService.GetPatientById(patientId);

        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        //Console.Write("Enter doctor specialisation: ");
        var specialization = GetSpecialisationFromUser();

        var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

        if (!doctors.Any())
        {
            Console.WriteLine("No doctors found for this specialisation.");
            return;
        }
        Console.WriteLine("\nAvailable Doctors:");
        foreach (var d in doctors)
        {
            Console.WriteLine($"ID: {d.DoctorId}, Name: {d.FullName}, Exp: {d.YearsOfExperience} yrs, Fee: {d.ConsultationFee}");
        }
        Console.Write("\nChoose Doctor ID: ");
        int doctorId = int.Parse(Console.ReadLine() ?? "0");

        var doctor = doctorService.GetById(doctorId);

        if (doctor == null)
        {
            Console.WriteLine("Invalid doctor selection.");
            return;
        }

        Console.Write("Appointment date yyyy-MM-dd: ");
        DateTime date = DateTime.Parse(Console.ReadLine() ?? string.Empty);
        if (date < DateTime.Now.AddMonths(6))
        {
            var appointment = appointmentService.BookAppointment(patient, doctor, date);
            var allAppointments = appointmentService.GetAllAppointments();
            Console.WriteLine("\nAppointment booked successfully.");
            Console.WriteLine($"Assigned Slot: {appointment.TimeSlot}");
            Console.WriteLine(appointment.GetDetails(allAppointments));
        }
        else
        {
            Console.WriteLine("Appointments can only be booked within 6 months from today.");
        }

        
    }
    catch (PastDateException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (DoctorUnavailableException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
}
//5th Function

void ViewAppointmentsForPatient()
{
    Console.Write("Enter Patient ID: ");
    int patientId = int.Parse(Console.ReadLine() ?? "0");

    var patient = patientService.GetPatientById(patientId);

    if (patient == null)
    {
        Console.WriteLine("Patient not found.");
        return;
    }

    var appointments = appointmentService.GetAppointmentsByPatient(patientId);

    if (!appointments.Any())
    {
        Console.WriteLine("No appointments found for this patient.");
        return;
    }

    Console.WriteLine($"\nAppointments for {patient.FullName}:\n");

    foreach (var appointment in appointments)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Appointment ID : {appointment.AppointmentId}");
        Console.WriteLine($"Doctor         : {appointment.Doctor.FullName} ({appointment.Doctor.Specialisation})");
        Console.WriteLine($"Date           : {appointment.ScheduledDate:yyyy-MM-dd}");
        Console.WriteLine($"Time Slot      : {appointment.TimeSlot}");
        Console.WriteLine($"Status         : {appointment.Status}");
        Console.WriteLine($"Cancellation   : {(string.IsNullOrWhiteSpace(appointment.CancellationReason) ? "N/A" : appointment.CancellationReason)}");
    }

    Console.WriteLine("----------------------------------------");
}

//6th Function
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

        Console.Write($"We have your Apponintmentwith Id {appointmentId}.\n Please choose the below option to make changes to the status of your Appointment.");
        Console.WriteLine("Press 1 to Cancel your appointment");
        Console.WriteLine("Press 2 to Complete your appointrment");

        string action = Console.ReadLine() ?? string.Empty;

        if (action == "1")
        {
            Console.Write("Cancellation reason: ");
            string reason = Console.ReadLine() ?? string.Empty;
            appointmentService.CancelAppointment(appointmentId, reason);
            Console.WriteLine("Appointment cancelled.");
        }
        else if (action == "2")
        {
            appointment.Status = Appointment.StatusOption.Completed;
            Console.WriteLine("Appointment completed.");
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }

        var allAppointments = appointmentService.GetAllAppointments();
        Console.WriteLine(appointment.GetDetails(allAppointments));
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");

    }
}

//7th Function
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

    if (appointment.Status != Appointment.StatusOption.Completed)
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

//8th Function
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
        Console.WriteLine(record.GetSummary());
        Console.WriteLine("-----------------------------------");
    }
}
//9th Function
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
//10th Function
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

//11th Function

void ViewUpcomingConfirmedAppointments()
{
    var appointments = appointmentService.GetUpcomingAppointments();

    if (!appointments.Any())
    {
        Console.WriteLine(" No upcoming confirmed appointments found.");
        return;
    }

    Console.WriteLine("\nUpcoming Confirmed Appointments:\n");

    foreach (var appointment in appointments)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Appointment ID : {appointment.AppointmentId}");
        Console.WriteLine($"Patient        : {appointment.Patient.FullName}");
        Console.WriteLine($"Doctor         : {appointment.Doctor.FullName} ({appointment.Doctor.Specialisation})");
        Console.WriteLine($"Date           : {appointment.ScheduledDate:yyyy-MM-dd}");
        Console.WriteLine($"Time Slot      : {appointment.TimeSlot}");
        Console.WriteLine($"Status         : {appointment.Status}");
    }

    Console.WriteLine("----------------------------------------");
}

//12th Function
void MakeDoctorActive()
{
    Console.Write("Enter Doctor ID: ");

    if (!int.TryParse(Console.ReadLine(), out int doctorId))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var doctor = doctorService.GetById(doctorId);

    if (doctor == null)
    {
        Console.WriteLine("Doctor not found.");
        return;
    }

    Console.WriteLine($"Doctor: {doctor.FullName}");
    Console.WriteLine("1. Activate");
    Console.WriteLine("2. Deactivate");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    if (choice == "1")
    {
        doctor.IsActive = true;
        Console.WriteLine("Doctor is now ACTIVE.");
    }
    else if (choice == "2")
    {
        doctor.IsActive = false;
        Console.WriteLine("Doctor is now INACTIVE.");
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}