using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Exceptions;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using HAP_Pod4_ConsoleApp_au.Repository;
using HAP_Pod4_ConsoleApp_au.Services.Impl;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;


AppDbContext context = new AppDbContext();

IAppointmentRepository appointmentRepository = new AppointmentRepository();

IPatientRepository patientRepository = new PatientRepository();

IDoctorRepository doctorRepository = new DoctorRepository();

IHealthRepository healthRepository = new HealthRepository();

PatientService patientService = new PatientService(patientRepository);

AppointmentService appointmentService = new AppointmentService(appointmentRepository);

DoctorService doctorService = new DoctorService(context);

HealthRecordService healthRecordService = new HealthRecordService(healthRepository);


SeedRepositories(context, patientRepository, doctorRepository);

static void SeedRepositories(AppDbContext context, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
{
    foreach (var patient in context.Patients)
    {
        patientRepository.RegisterPatient(patient);
    }

    foreach (var doctor in context.Doctors)
    {
        doctorRepository.AddDoctor(doctor);
    }
}

bool exit = false;

while (!exit)
{
    Console.WriteLine("\n===== HEALTH APPOINTMENT PORTAL =====");
    Console.WriteLine("1. Patient Menu");
    Console.WriteLine("2. Doctor Menu");
    //Console.WriteLine("3. Clear Screen");
    Console.WriteLine("3. Exit");
    Console.Write("Enter choice: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            PatientMenu(
                patientService,
                appointmentService,
                doctorService,
                healthRecordService,
                context);
            break;

        case "2":
            DoctorMenu(
                doctorService,
                appointmentService,
                healthRecordService,
                context);
            break;

        //case "3":
        //    Console.Clear();
        //    break;

        case "3":
            exit = true;
            Console.WriteLine("Application Closed.");
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

static void PatientMenu(
    PatientService patientService,
    AppointmentService appointmentService,
    DoctorService doctorService,
    HealthRecordService healthRecordService,
    AppDbContext context)
{
    bool back = false;
    Console.Clear();

    while (!back)
    {
        Console.WriteLine("\n===== PATIENT MENU =====");
        Console.WriteLine("1. Register New Patient");
        Console.WriteLine("2. View All Patients");
        Console.WriteLine("3. Search Doctors By Specialisation");
        Console.WriteLine("4. Book Appointment");
        Console.WriteLine("5. View Patient Appointments");
        Console.WriteLine("6. Cancel Appointment");
        Console.WriteLine("7. View Health History");
        Console.WriteLine("8. Back");
        Console.Write("Enter choice: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                RegisterPatient(patientService, context);
                break;

            case "2":
                ViewAllPatients(patientService);
                break;

            case "3":
                SearchDoctors(doctorService);
                break;

            case "4":
                BookAppointment(
                    appointmentService,
                    patientService,
                    doctorService,
                    context);
                break;

            case "5":
                ViewAppointments(
                    appointmentService,
                    patientService);
                break;

            case "6":
                CancelAppointmentByPatient(appointmentService);
                break;

            case "7":
                ViewHealthHistory(
                    healthRecordService,
                    patientService);
                break;

            case "8":
                back = true;
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

static void DoctorMenu(
    DoctorService doctorService,
    AppointmentService appointmentService,
    HealthRecordService healthRecordService,
    AppDbContext context)
{
    bool back = false;
    Console.Clear();

    while (!back)
    {
        Console.WriteLine("\n===== DOCTOR MENU =====");
        Console.WriteLine("1. Add New Doctor");
        Console.WriteLine("2. View All Doctors");
        Console.WriteLine("3. Search Doctors By Specialisation");
        Console.WriteLine("4. View Upcoming Appointments");
        Console.WriteLine("5. Confirm Or Cancel Appointment");
        Console.WriteLine("6. Add Health Record");
        Console.WriteLine("7. Back");

        Console.Write("Enter choice: ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddDoctor(doctorService);
                break;

            case "2":
                ViewAllDoctors(doctorService);
                break;

            case "3":
                SearchDoctors(doctorService);
                break;

            case "4":
                ViewUpcomingAppointments(appointmentService);
                break;

            case "5":
                ManageAppointment(appointmentService);
                break;

            case "6":
                AddHealthRecord(
                    appointmentService,
                    healthRecordService,
                    context);
                break;

            case "7":
                back = true;
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
}

//bool exit = false;

//while (!exit)
//{
//    Console.WriteLine("\n===== HEALTH APPOINTMENT PORTAL =====");
//    Console.WriteLine("1. Register New Patient");
//    Console.WriteLine("2. Add New Doctor");
//    Console.WriteLine("3. Search Doctors By Specialisation");
//    Console.WriteLine("4. Book Appointment");
//    Console.WriteLine("5. View Patient Appointments");
//    Console.WriteLine("6. Confirm Or Cancel Appointment");
//    Console.WriteLine("7. Add Health Record After Completed Appointment");
//    Console.WriteLine("8. View Health History For Patient");
//    Console.WriteLine("9. View all patients");
//    Console.WriteLine("10. View all doctors");
//    Console.WriteLine("11. View upcoming appointments");
//    Console.WriteLine("12. Clear");
//    Console.WriteLine("13. Exit");
//    Console.Write("Enter choice: ");

//    string? choice = Console.ReadLine();

//    switch (choice)
//    {
//        case "1":
//            RegisterPatient(patientService, context);
//            break;

//        case "2":
//            AddDoctor(doctorService);
//            break;

//        case "3":
//            SearchDoctors(doctorService);
//            break;

//        case "4":
//            BookAppointment(appointmentService,patientService,doctorService,context);
//            break;

//        case "5":
//            ViewAppointments(
//                appointmentService,
//                patientService);
//            break;

//        case "6":
//            ManageAppointment(appointmentService);
//            break;

//        case "7":
//            AddHealthRecord(
//                appointmentService,
//                healthRecordService,
//                context);
//            break;

//        case "8":
//            ViewHealthHistory(healthRecordService, patientService);
//            break;

//        case "9":
//            ViewAllPatients(patientService);
//            break;

//        case "10":
//            ViewAllDoctors(doctorService);
//            break;

//        case "11":
//            ViewUpcomingAppointments(appointmentService);
//            break;

//        case "12":
//            Console.Clear();
//            break;

//        case "13":
//            exit = true;
//            Console.WriteLine("Application Closed.");
//            break;

//        default:
//            Console.WriteLine("Invalid option.");
//            break;
//    }
//}

static void RegisterPatient(PatientService patientService, AppDbContext context)
{
    try
    {
        Console.Write("Enter Full Name: ");
        string name = ReadValidName();

        Console.Write("Enter Date Of Birth (dd-mm-yyyy): ");
        DateTime dob = ReadValidDOB();

        Console.WriteLine("Select Gender:");
        Console.WriteLine("Male");
        Console.WriteLine("Female");
        Console.WriteLine("Transgender");
        Console.WriteLine("Other");
        Console.WriteLine();

        Patient.GenderOptions gender;

        while (true)
        {
            Console.Write("Enter Gender: ");

            string? input = Console.ReadLine();

            if (Enum.TryParse<Patient.GenderOptions>(
                    input,
                    true,
                    out gender))
            {
                break;
            }

            Console.WriteLine(
                "Invalid Gender. \nPlease select from above options: ");
        }

        Console.Write("Enter Phone Number: ");
        string phone = ReadValidPhone();

        Console.Write("Enter Email: ");
        string email = ReadValidEmail();

        Console.Write("Enter Insurance ID: ");
        string insuranceId = Console.ReadLine() ?? string.Empty;

        Patient patient = new Patient
        {
            PatientId = context.GetNextPatientId(),
            FullName = name,
            DateOfBirth = dob,
            Gender = (Patient.GenderOptions)gender,
            PhoneNumber = phone,
            Email = email,
            InsuranceID = insuranceId,
            CreatedDate = DateTime.Now
        };

        patientService.RegisterPatient(patient);

        Console.WriteLine("Patient Registered Successfully.");
        Console.WriteLine(patient.GetProfileSummary());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void AddDoctor(DoctorService doctorService)
{
    try
    {
        Console.Write("Enter Doctor Name: ");
        string name = ReadValidName();

        Console.WriteLine("Select Specialisation:");

        foreach (Doctor.SpecialisationOption spec in Enum.GetValues<Doctor.SpecialisationOption>())
        {
            Console.WriteLine($"{(int)spec + 1} - {spec}");
        }

        int specChoice = ReadPositiveInteger();

        Console.Write("Enter Years Of Experience: ");
        int experience = ReadPositiveInteger();

        Console.Write("Enter Consultation Fee: ");
        int fee = ReadPositiveInteger();

        Doctor doctor = new Doctor
        {
            FullName = name,
            Specialisation =
                (Doctor.SpecialisationOption)specChoice,
            YearsOfExperience = experience,
            ConsultationFee = fee,
            IsActive = true
        };

        doctorService.AddDoctor(doctor);

        Console.WriteLine("Doctor Added Successfully.");
        Console.WriteLine(doctor.GetProfileSummary());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void SearchDoctors(DoctorService doctorService)
{
    Console.WriteLine("Select Specialisation:");

    foreach (Doctor.SpecialisationOption spec in Enum.GetValues<Doctor.SpecialisationOption>())
    {
        Console.WriteLine($"{(int)spec} - {spec}");
    }

    int specChoice = ReadPositiveInteger();

    var doctors = doctorService.SearchDoctorBySpecialisation((Doctor.SpecialisationOption)specChoice);

    if (doctors.Count == 0)
    {
        Console.WriteLine("No Doctors Found.");
        return;
    }
    Console.WriteLine();

    foreach (var doctor in doctors)
    {
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        Console.WriteLine(doctor.GetProfileSummary());
        //Console.WriteLine();
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        Console.WriteLine();
    }
}

static void BookAppointment(
    AppointmentService appointmentService,
    PatientService patientService,
    DoctorService doctorService,
    AppDbContext context)
{
    try
    {
        Console.Write("Enter Patient ID: ");
        int patientId = ReadPositiveInteger();

        var patient = patientService.GetPatientById(patientId);

        if (patient == null)
        {
            Console.WriteLine("Patient Not Found.");
            return;
        }

        Console.WriteLine("Available Doctors:");

        Console.WriteLine("-------------------------------------------");
        foreach (var doc in doctorService.GetAllDoctors())
        {
            Console.WriteLine(doc.GetProfileSummary());
            Console.WriteLine("-------------------------------------------");

        }

        Console.Write("Enter Doctor ID: ");
        int doctorId = ReadPositiveInteger();

        var doctor = doctorService.GetAllDoctors()
            .FirstOrDefault(d => d.DoctorId == doctorId);

        if (doctor == null)
        {
            Console.WriteLine("Doctor Not Found.");
            return;
        }

        Console.Write("Enter Appointment Date (dd-mm-yyyy): ");
        DateTime appointmentDate = ReadValidDate();

        Console.WriteLine("Select Time Slot:");

        foreach (Appointment.TimeSlotOption slot in
         Enum.GetValues<Appointment.TimeSlotOption>())
        {
            Console.WriteLine($"{(int)slot}. {slot}");
        }

        int slotChoice = ReadPositiveInteger();

        Appointment appointment = new()
        {
            AppointmentId = context.GetNextAppointmentId(),
            Patient = patient,
            Doctor = doctor,
            ScheduledDate = appointmentDate,
            TimeSlot =
                (Appointment.TimeSlotOption)slotChoice,
            Status = Appointment.StatusOption.Pending
        };

        appointmentService.BookAppointment(appointment);

        Console.WriteLine("Appointment Booked Successfully.");
        Console.WriteLine(appointment.GetDetails());
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Conflict: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void ViewAppointments(
    AppointmentService appointmentService,
    PatientService patientService)
{
    Console.Write("Enter Patient ID: ");
    int patientId = ReadPositiveInteger();

    var patient = patientService.GetPatientById(patientId);

    if (patient == null)
    {
        Console.WriteLine("Patient Not Found.");
        return;
    }

    var appointments =
        appointmentService.GetAppointmentsBypatient(patientId);

    if (appointments.Count == 0)
    {
        Console.WriteLine("No Appointments Found.");
        return;
    }

    foreach (var appointment in appointments)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(appointment.GetDetails());
    }
}

static void ManageAppointment(
    AppointmentService appointmentService)
{
    Console.Write("Enter Appointment ID: ");
    int appointmentId = ReadPositiveInteger();

    var appointment =
        appointmentService.GetAppointmentById(appointmentId);

    if (appointment == null)
    {
        Console.WriteLine("Appointment Not Found.");
        return;
    }

    Console.WriteLine("1. Confirm Appointment");
    Console.WriteLine("2. Cancel Appointment");
    Console.WriteLine("3. Complete Appointment");

    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            appointment.Confirm();
            Console.WriteLine("Appointment Confirmed.");
            break;

        case "2":
            Console.Write("Enter Cancellation Reason: ");

            string reason =
                Console.ReadLine() ?? string.Empty;

            bool isCancelled =
                appointmentService.CancelAppointment(appointmentId, reason);

            if (isCancelled)
            {
                Console.WriteLine(
                    "Appointment Cancelled.");
            }

            break;

        case "3":
            appointment.Complete();
            Console.WriteLine("Appointment Completed.");
            break;

        default:
            Console.WriteLine("Invalid Choice.");
            break;
    }
}

static void AddHealthRecord(
    AppointmentService appointmentService,
    HealthRecordService healthRecordService,
    AppDbContext context)
{
    try
    {
        Console.Write("Enter Appointment ID: ");
        int appointmentId = ReadPositiveInteger();

        var appointment =
            appointmentService.GetAppointmentById(appointmentId);

        if (appointment == null)
        {
            Console.WriteLine("Appointment Not Found.");
            return;
        }

        if (appointment.Status !=
            Appointment.StatusOption.Completed)
        {
            Console.WriteLine("Health record can only be added after appointment completion.");
            return;
        }

        Console.Write("Enter Diagnosis: ");
        string diagnosis = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Prescription: ");
        string prescription = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter Notes: ");
        string notes = Console.ReadLine() ?? string.Empty;

        if (appointment.Patient == null || appointment.Doctor == null)
        {
            Console.WriteLine("Invalid appointment data.");
            return;
        }

        HealthRecord record = new HealthRecord
        {
            RecordId = context.GetNextHealthRecordId(),
            Patient = appointment.Patient,
            Doctor = appointment.Doctor,
            VisitDate = DateTime.Now,
            Diagnosis = diagnosis,
            Prescription = prescription,
            Notes = notes
        };

        healthRecordService.AddRecord(record);

        Console.WriteLine("Health Record Added Successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

static void ViewHealthHistory(
    HealthRecordService healthRecordService,
    PatientService patientService)
{
    Console.Write("Enter Patient ID: ");
    int patientId = ReadPositiveInteger();

    var patient = patientService.GetPatientById(patientId);

    if (patient == null)
    {
        Console.WriteLine("Patient Not Found.");
        return;
    }

    var records =
        healthRecordService.GetRecordsByPatient(patientId);

    if (records.Count == 0)
    {
        Console.WriteLine("No Health Records Found.");
        return;
    }

    foreach (var record in records)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(record.GetSummary());
    }
}

static string ReadValidName()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input) &&
            Regex.IsMatch(input, "^[a-zA-Z .]+$"))
        {
            return input;
        }

        Console.Write("Invalid Name. Re-enter: ");
    }
}

static string ReadValidPhone()
{
    while (true)
    {
        string? input = Console.ReadLine();

#pragma warning disable SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.
        if (!string.IsNullOrWhiteSpace(input) &&
            Regex.IsMatch(input, "^[6-9][0-9]{9}$"))
        {
            return input;
        }
#pragma warning restore SYSLIB1045 // Convert to 'GeneratedRegexAttribute'.

        Console.Write("Invalid Phone Number. \n Please Re-enter: ");
    }
}

static string ReadValidEmail()
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(input) && input.Contains('@') && input.Contains('.'))
        {
            return input;
        }

        Console.Write("Invalid Email. \nRe-enter: ");
    }
}

static DateTime ReadValidDOB()
{
    while (true)
    {
        if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
        {
            if (dob < DateTime.Today)
            {
                return dob;
            }
        }

        Console.Write("Invalid DOB. \nPlease Re-enter: ");
    }
}


static DateTime ReadValidDate()
{
    while (true)
    {
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            Console.Write("Invalid Date. \nKindly Re-enter: ");
            continue;
        }

        if (date.Date <= DateTime.Today)
        {
            Console.Write("Appointment date must be in the future. \nKindly Re-enter: ");
            continue;
        }

        if (date.Date > DateTime.Today.AddMonths(3))
        {
            Console.Write("Book an appointment date within 3 months \nKindly Re-enter: ");
            continue;
        }

        return date;
    }
}
static int ReadPositiveInteger()
{
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int number)
            && number >= 0)
        {
            return number;
        }

        Console.Write("Invalid Number. \nRe-enter: ");
    }
}

// All Patients
static void ViewAllPatients(
    PatientService patientService)
{
    var patients = patientService.GetAllPatients();

    if (patients.Count == 0)
    {
        Console.WriteLine("No Patients Found.");
        return;
    }

    Console.WriteLine("\n===== ALL PATIENTS =====");

    foreach (var patient in patients)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(patient.GetProfileSummary());
    }
}


// All Doctors
static void ViewAllDoctors(
    DoctorService doctorService)
{
    var doctors = doctorService.GetAllDoctors();

    if (doctors.Count == 0)
    {
        Console.WriteLine("No Doctors Found.");
        return;
    }

    Console.WriteLine("\n===== ALL DOCTORS =====");

    foreach (var doctor in doctors)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(doctor.GetProfileSummary());
    }
}

// All Appointments

static void ViewUpcomingAppointments(
    AppointmentService appointmentService)
{
    var appointments =
        appointmentService.GetAllAppointments();

    var upcomingAppointments = appointments.Where(a => a.ScheduledDate > DateTime.Now && a.Status != Appointment.StatusOption.Cancelled).ToList();

    if (upcomingAppointments.Count == 0)
    {
        Console.WriteLine("No Upcoming Appointments Found.");
        return;
    }

    Console.WriteLine("\n===== UPCOMING APPOINTMENTS =====");

    foreach (var appointment in upcomingAppointments)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine(appointment.GetDetails());
    }
}

static void CancelAppointmentByPatient(
    AppointmentService appointmentService)
{
    Console.Write("Enter Appointment ID: ");

    int appointmentId = ReadPositiveInteger();

    var appointment =
        appointmentService.GetAppointmentById(appointmentId);

    if (appointment == null)
    {
        Console.WriteLine("Appointment Not Found.");
        return;
    }

    Console.Write("Enter Cancellation Reason: ");

    string reason =
        Console.ReadLine() ?? string.Empty;

    bool isCancelled =
        appointmentService.CancelAppointment(
            appointmentId,
            reason);

    if (isCancelled)
    {
        Console.WriteLine(
            "Appointment Cancelled Successfully.");
    }
    else
    {
        Console.WriteLine(
            "Unable To Cancel Appointment.");
    }
}

[ExcludeFromCodeCoverage]
public partial class Program { }