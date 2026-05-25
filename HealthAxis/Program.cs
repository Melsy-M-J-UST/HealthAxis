using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Repository.Implementation;
using HealthAxis.Service;
using HealthAxis.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

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
    Console.WriteLine("6. Cancel or Complete an appointment");
    Console.WriteLine("7. Add a health record after a completed appointment");
    Console.WriteLine("8. View health history for a patient");
    Console.WriteLine("9. View all patients");
    Console.WriteLine("10. View all doctors");
    Console.WriteLine("11. Update Portal");
    Console.WriteLine("12. Change Doctor status");
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
            //ConfirmCancelOrCompleteAppointment();
            break;
        case "7":
            //AddHealthRecord();
            break;
        case "8":
            //ViewHealthHistory();
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
            //ToggleDoctorActiveStatus();
            break;
        case "13":
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
    string FullName = Console.ReadLine() ?? string.Empty;

    if (Regex.IsMatch(
            FullName,
            @"^[A-Za-z]+( [A-Za-z]+)*$",
            RegexOptions.None,
            TimeSpan.FromMilliseconds(500)))
    {
        p.PatientName = FullName;
    }
    else
    {
        Console.WriteLine("Enter a Valid Name");
        return;
    }

    Console.Write("Enter your Date of Birth:\n");
    var inputDate = Console.ReadLine();
    if ((DateTime.TryParse(inputDate, out DateTime dob)) && (dob < DateTime.Today))
    {
        p.DateOfBirth = dob;
    }
    else
    {
        Console.WriteLine("Invalid date format. Please enter a valid date.");
        return;
    }

    Console.Write("Enter your Gender: \nMale\nFemale\nTransgender\nOther\nKindly please enter the one among the four given above.\n");
    bool G = Enum.TryParse(Console.ReadLine(), true, out Patient.Genders gender);
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

    while (!Regex.IsMatch(
               PhoneNumber,
               @"^\d{10}$",
               RegexOptions.None,
               TimeSpan.FromMilliseconds(300)))
    {
        Console.WriteLine("Enter a valid Phone Number");
        PhoneNumber = Console.ReadLine() ?? string.Empty;
    }
    p.PhoneNumber = PhoneNumber;

    Console.Write("Enter your Mail Id: ");
    string Email = Console.ReadLine() ?? string.Empty;
    if (Email != string.Empty)
    {
        if (Regex.IsMatch(
        Email,
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.None,
        TimeSpan.FromMilliseconds(500)))
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
    p.RegisteredDate = now;

    patientService.RegisterPatient(p);
    Console.WriteLine();
}
void AddDoctor()
{
    try
    {
        Doctor doctor = new Doctor();

        doctor.DoctorId = db.GetNextDoctorId();

        Console.Write("Enter Full Name: ");
        string FullName = Console.ReadLine() ?? string.Empty;

        if (Regex.IsMatch(
                FullName,
                @"^[A-Za-z]+( [A-Za-z]+)*$",
                RegexOptions.None,
                TimeSpan.FromMilliseconds(500)))
        {
            doctor.DoctorName = FullName;
        }
        else
        {
            Console.WriteLine("Enter a Valid Name");
            return;
        }

        doctor.Specialisation = GetSpecialisationFromUser();

        Console.Write("Enter Years of Experience: ");
        doctor.Experience = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Consultation Fee: ");
        doctor.Fees = Convert.ToInt32(Console.ReadLine());

        Console.Write("Is Active (true/false): ");
        doctor.IsPractising = Convert.ToBoolean(Console.ReadLine());

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
            Console.WriteLine(doctor.GetDoctorSummary());
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
Doctor.Specialisations GetSpecialisationFromUser()
{
    Console.WriteLine("Choose Specialisation:");

    var specialisations = Enum.GetValues(typeof(Doctor.Specialisations));

    for (int i = 0; i < specialisations.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {specialisations.GetValue(i)}");
    }

    Console.Write("Enter Specialisation: ");
    string input = Console.ReadLine() ?? string.Empty;


    if (int.TryParse(input, out int choice))
    {
        if (choice >= 1 && choice <= specialisations.Length)
        {
            return (Doctor.Specialisations)specialisations.GetValue(choice - 1)!;
        }
    }

    else if (Enum.TryParse(input, true, out Doctor.Specialisations result))
    {
        return result;
    }

    throw new Exception("Invalid Specialisation Entered");
}
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
            Console.WriteLine($"ID: {d.DoctorId}, Name: Dr. {d.DoctorName}, Exp: {d.Experience} yrs, Fee: {d.Fees}");
        }
        Console.Write("\nChoose Doctor ID: ");
        int doctorId = int.Parse(Console.ReadLine() ?? "0");

        var doctor = doctorService.GetDoctorById(doctorId);

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
            Console.WriteLine($"Assigned Slot: {appointment.Slot}");
            Console.WriteLine(appointment.GetAppointmentSummary(allAppointments));
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

    Console.WriteLine($"\nAppointments for {patient.PatientName}:\n");

    foreach (var appointment in appointments)
    {
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Appointment ID : {appointment.AppointmentId}");
        Console.WriteLine($"Doctor         : {appointment.Doctor.DoctorName} ({appointment.Doctor.Specialisation})");
        Console.WriteLine($"Date           : {appointment.ScheduledDate:yyyy-MM-dd}");
        Console.WriteLine($"Time Slot      : {appointment.Slot}");
        Console.WriteLine($"Status         : {appointment.Status}");
        Console.WriteLine($"Cancellation   : {(string.IsNullOrWhiteSpace(appointment.CancellationReason) ? "N/A" : appointment.CancellationReason)}");
    }

    Console.WriteLine("----------------------------------------");
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
        Console.WriteLine(patient.GetPatientSummary());
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
        Console.WriteLine(doctor.GetDoctorSummary());
    }
}