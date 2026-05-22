using HealthAxis.Data;
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
            //RegisterPatient();
            break;
        case "2":
            //AddDoctor();
            break;
        case "3":
            //SearchDoctorsBySpecialisation();
            break;
        case "4":
            //BookAppointment();
            break;
        case "5":
            //ViewAppointmentsForPatient();
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
            //ViewAllPatients();
            break;
        case "10":
            //ViewAllDoctors();
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
    string FullName = Console.ReadLine() ?? string.Empty;
    if (Regex.IsMatch(FullName, @"^[A-Za-z]+( [A-Za-z]+)*$"))
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
    while (!Regex.IsMatch(PhoneNumber, @"^\d{10}$"))
    {
        Console.WriteLine("Enter a valid Phone Number");
        PhoneNumber = Console.ReadLine() ?? string.Empty;
    }
    p.PhoneNumber = PhoneNumber;

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
    p.RegisteredDate = now;

    patientService.RegisterPatient(p);
    Console.WriteLine();
}