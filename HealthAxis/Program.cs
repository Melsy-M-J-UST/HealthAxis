using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repository;
using HealthAxis.Repository.Implementation;
using HealthAxis.Service;
using HealthAxis.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
            CancelOrCompleteAppointment();
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
            Update();
            break;
        case "12":
            ToggleDoctorStatus();
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
    Patient p = new();

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
    string? inputDate = Console.ReadLine();
    if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob) && dob < DateTime.Today)
    {
        p.DateOfBirth = dob;
    }
    else
    {
        Console.WriteLine("Invalid date format. Please enter the date in \"yyyy-MM-dd\" Format.");
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
    p.PatientId = db.GetNextPatientId();
    patientService.RegisterPatient(p);
    Console.WriteLine();
}
void AddDoctor()
{
    try
    {
        Doctor doctor = new();

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

        doctor.DoctorId = db.GetNextDoctorId();

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

    var specialisations = Enum.GetValues<Doctor.Specialisations>();

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

        if (doctors.Count==0)
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

        DateTime date = DateTime.ParseExact(Console.ReadLine() ?? string.Empty, "yyyy-MM-dd", CultureInfo.InvariantCulture);

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

    if (appointments.Count==0)
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
void CancelOrCompleteAppointment()
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

    HealthRecord record = new()
    {
        Patient = appointment.Patient,
        Doctor = appointment.Doctor,
        VisitedDate = appointment.ScheduledDate
    };

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

    if (records == null || records.Count==0)
    {
        Console.WriteLine("No health records found for this patient.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Health History =====");

    foreach (var record in records)
    {
        Console.WriteLine(record.GetHealthRecordSummary());
        Console.WriteLine("-----------------------------------");
    }
}

void ViewAllPatients()
{
    var patients = patientService.GetAllPatients();
    if (patients.Count == 0)
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
    if (doctors.Count == 0)
    {
        Console.WriteLine("No Doctors Found");
        return;
    }
    foreach (var doctor in doctors)
    {
        Console.WriteLine(doctor.GetDoctorSummary());
    }
}
void Update()
{
    Console.WriteLine("==================Updation===================");
    Console.WriteLine("1.To Update Patient");
    Console.WriteLine("2.To Update Doctor");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            UpdatePatient();
            break;
        case "2":
            UpdateDoctor();
            break;
        default:
            Console.WriteLine("Invalid Choice");
            break;
    }
    void UpdatePatient()
    {
        ViewAllPatients();
        Console.WriteLine("\n");
        Console.Write("Enter Patient ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        var patient = patientService.GetPatientById(id);

        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        Console.WriteLine("Press ENTER to keep existing values");

        Console.Write($"Name ({patient.PatientName}): ");
        string name = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(name))
            patient.PatientName = name;

        Console.Write($"Phone ({patient.PhoneNumber}): ");
        string phone = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(phone))
            patient.PhoneNumber = phone;

        Console.Write($"Email ({patient.Email}): ");
        string email = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(email))
            patient.Email = email;

        Console.WriteLine($@"Current Gender: {patient.Gender}
            Enter your Gender:
            Male
            Female
            Transgender
            Other
            (Press ENTER to keep existing)");

        string input = Console.ReadLine()!;

        if (!string.IsNullOrWhiteSpace(input))
        {
            bool isValid = Enum.TryParse(input, true, out Patient.Genders gender);

            if (isValid)
            {
                patient.Gender = gender;
            }
            else
            {
                Console.WriteLine("Enter valid gender from the list");
                return;
            }
        }
        Console.Write($"DOB ({patient.DateOfBirth:yyyy-MM-dd}): ");
        string dobInput = Console.ReadLine()!;
        if (DateTime.TryParseExact(dobInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
        {
            patient.DateOfBirth = dob;
        }
        else
        {
            Console.WriteLine("Invalid date format. Use yyyy-MM-dd.");
            return;
        }
        var result = patientService.UpdatePatient(patient);
        Console.WriteLine(result ? "Patient updated " : "Update failed ");
    }
    void UpdateDoctor()
    {
        ViewAllDoctors();
        Console.WriteLine("\n");
        Console.Write("Enter Doctor ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        var doctor = doctorService.GetDoctorById(id);

        if (doctor == null)
        {
            Console.WriteLine("Doctor not found.");
            return;
        }
        Console.WriteLine("Press ENTER to keep existing values");
        Console.Write($"Name ({doctor.DoctorName}): ");
        string name = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(name))
            doctor.DoctorName = name;
        Console.Write($"Specialisation ({doctor.Specialisation}): ");
        string specInput = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(specInput))
        {
            var specialisations = Enum.GetValues<Doctor.Specialisations>();
            if (int.TryParse(specInput, out int specChoice))
            {
                if (specChoice >= 1 && specChoice <= specialisations.Length)
                {
                    doctor.Specialisation = (Doctor.Specialisations)specialisations.GetValue(specChoice - 1)!;
                }
                else
                {
                    Console.WriteLine("Invalid Specialisation choice.");
                    return;
                }
            }
            else if (Enum.TryParse(specInput, true, out Doctor.Specialisations specEnum))
            {
                doctor.Specialisation = specEnum;
            }
            else
            {
                Console.WriteLine("Invalid Specialisation.");
                return;
            }
        }
        Console.Write($"Experience ({doctor.Experience}): ");
        string expInput = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(expInput))
            doctor.Experience = int.Parse(expInput);
        Console.Write($"Fee ({doctor.Fees}): ");
        string feeInput = Console.ReadLine()!;
        if (!string.IsNullOrWhiteSpace(feeInput))
            doctor.Fees = int.Parse(feeInput);
        var result = doctorService.UpdateDoctor(doctor);
        Console.WriteLine(result ? "Doctor updated" : "Update failed");
    }
}
void ToggleDoctorStatus()
{
    ViewAllDoctors();
    Console.Write("Enter Doctor ID: ");

    if (!int.TryParse(Console.ReadLine(), out int doctorId))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var doctor = doctorService.GetDoctorById(doctorId);

    if (doctor == null)
    {
        Console.WriteLine("Doctor not found.");
        return;
    }

    Console.WriteLine($"Doctor: {doctor.DoctorName}");
    Console.WriteLine("1. Activate");
    Console.WriteLine("2. Deactivate");
    Console.Write("Choose an option: ");

    var choice = Console.ReadLine();

    if (choice == "1")
    {
        doctor.IsPractising = true;
        Console.WriteLine("Doctor is now ACTIVE.");
    }
    else if (choice == "2")
    {
        doctor.IsPractising = false;
        Console.WriteLine("Doctor is now INACTIVE.");
    }
    else
    {
        Console.WriteLine("Invalid choice.");
    }
}

[ExcludeFromCodeCoverage]
public static partial class Program
{
}