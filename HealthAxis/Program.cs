using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Repositories.Impl;
using HealthAxis.Services;
using HealthAxis.Services.Impl;
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
    try
    {
        Console.WriteLine();
        Console.WriteLine("===== Appointment Portal =====");
        Console.WriteLine("1. Register a new patient");
        Console.WriteLine("2. Add a new doctor");
        Console.WriteLine("3. Search doctors by specialisation");
        Console.WriteLine("4. Book an appointment for a patient");
        Console.WriteLine("5. View all appointments for a patient");
        Console.WriteLine("6. Cancel, or Complete an appointment");
        Console.WriteLine("7. Add a health record after a completed appointment");
        Console.WriteLine("8. View health history for a patient");
        Console.WriteLine("9. View all patients");
        Console.WriteLine("10. View all doctors");
        Console.WriteLine("11. Update Portal");
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
                Update();
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
    catch (Exception ex)
    {
        Console.WriteLine($"Unexpected application error: {ex.Message}");
    }
}

void RegisterPatient()
{
    try
    {
        Patient p = new Patient();

        Console.Write("Enter your full name: ");
        string fullName = Console.ReadLine() ?? string.Empty;

        if (Regex.IsMatch(fullName, @"^[A-Za-z]+( [A-Za-z]+)*$",RegexOptions.None,TimeSpan.FromMilliseconds(500)))
        {
            p.FullName = fullName;
        }
        else
        {
            throw new ArgumentException("Enter a valid name.");
        }

        Console.Write("Enter your Date of Birth(YYYY-MM-DD): ");
        var dateOfBirthInput = Console.ReadLine();

        if (!DateTime.TryParse(dateOfBirthInput, out DateTime dateOfBirth) || dateOfBirth > DateTime.Today)
        {
            throw new ArgumentException("Enter a valid Date of Birth.");
        }

        p.DateOfBirth = dateOfBirth;

        Console.WriteLine("Enter your Gender:");
        Console.WriteLine("Male");
        Console.WriteLine("Female");
        Console.WriteLine("Transgender");
        Console.WriteLine("Other");
        Console.Write("Kindly please enter one among the four given above: ");

        bool isGenderValid = Enum.TryParse(Console.ReadLine(), true, out Patient.GenderOptions gender);

        if (isGenderValid)
        {
            p.Gender = gender;
        }
        else
        {
            throw new ArgumentException("Enter valid gender from the list.");
        }

        Console.Write("Enter your Phone number: ");
        string phoneNumber = Console.ReadLine() ?? string.Empty;

        if (Regex.IsMatch(phoneNumber, @"^\d{10}$",RegexOptions.None,TimeSpan.FromMilliseconds(500)))
        {
            p.PhoneNumber = phoneNumber;
        }
        else
        {
            throw new ArgumentException("Enter a valid phone number.");
        }

        Console.Write("Enter your Mail Id: ");
        string email = Console.ReadLine() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
            {
                p.Email = email;
            }
            else
            {
                throw new ArgumentException("Enter a valid email id.");
            }
        }

        Console.Write("Enter your Insurance ID (optional): ");
        p.InsuranceID = Console.ReadLine() ?? string.Empty;

        p.CreatedDate = DateTime.Now;
        p.PatientId = db.GetNextPatientId();

        patientService.RegisterPatient(p);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while registering patient: {ex.Message}");
    }
}
void AddDoctor()
{
    try
    {
        Doctor doctor = new Doctor();

        Console.Write("Enter Full Name: ");
        string fullName = Console.ReadLine() ?? string.Empty;

        if (Regex.IsMatch(fullName, @"^[A-Za-z]+( [A-Za-z]+)*$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
        {
            doctor.FullName = fullName;
        }
        else
        {
            throw new ArgumentException("Enter a valid name.");
        }

        doctor.Specialisation = GetSpecialisationFromUser();

        Console.Write("Enter Years of Experience: ");
        if (!int.TryParse(Console.ReadLine(), out int experience) || experience < 0)
        {
            throw new FormatException("Enter a valid years of experience.");
        }

        doctor.YearsOfExperience = experience;

        Console.Write("Enter Consultation Fee: ");
        if (!int.TryParse(Console.ReadLine(), out int fee) || fee < 0)
        {
            throw new FormatException("Enter a valid consultation fee.");
        }

        doctor.ConsultationFee = fee;

        Console.Write("Is Active (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool isActive))
        {
            throw new FormatException("Enter valid active status as true or false.");
        }

        doctor.IsActive = isActive;

        doctor.DoctorId = db.GetNextDoctorId();
        doctorService.AddDoctor(doctor);

        Console.WriteLine("Doctor added successfully!");
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while adding doctor: {ex.Message}");
    }
}
void SearchDoctorsBySpecialisation()
{
    try
    {
        var specialization = GetSpecialisationFromUser();

        var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

        if (doctors == null || !doctors.Any())
        {
            Console.WriteLine("No doctors found for this specialisation.");
            return;
        }

        foreach (var doctor in doctors)
        {
            Console.WriteLine(doctor.GetProfileSummary());
        }
    }
    catch (DoctorNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while searching doctors: {ex.Message}");
    }
}

Doctor.SpecialisationOption GetSpecialisationFromUser()
{
    try
    {
        Console.WriteLine("Choose Specialisation:");

        var specialisations = Enum.GetValues(typeof(Doctor.SpecialisationOption));

        for (int i = 0; i < specialisations.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {specialisations.GetValue(i)}");
        }

        Console.Write("Enter Specialisation Name or Number: ");
        string specialisationInput = Console.ReadLine() ?? string.Empty;

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

        throw new ArgumentException("Invalid specialisation entered.");
    }
    catch
    {
        throw;
    }
}
void BookAppointment()
{
    try
    {
        Console.Write("Patient ID: ");

        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            throw new FormatException("Invalid patient ID.");
        }

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
            Console.WriteLine("No doctors found for this specialisation.");
            return;
        }

        Console.WriteLine("\nAvailable Doctors:");

        foreach (var d in doctors)
        {
            Console.WriteLine($"ID: {d.DoctorId}, Name: Dr. {d.FullName}, Exp: {d.YearsOfExperience} yrs, Fee: {d.ConsultationFee}");
        }

        Console.Write("\nChoose Doctor ID: ");

        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            throw new FormatException("Invalid doctor ID.");
        }

        var doctor = doctorService.GetById(doctorId);

        if (doctor == null)
        {
            Console.WriteLine("Invalid doctor selection.");
            return;
        }

        if (!doctor.IsActive)
        {
            Console.WriteLine("Selected doctor is inactive. Please choose another doctor.");
            return;
        }

        Console.Write("Appointment date yyyy-MM-dd: ");

        if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
        {
            throw new FormatException("Invalid appointment date.");
        }

        if (date < DateTime.Today)
        {
            throw new PastDateException("Appointment date cannot be in the past.");
        }

        if (date < DateTime.Now.AddMonths(6))
        {
            var appointment = appointmentService.BookAppointment(patient, doctor, date);
            var allAppointments = appointmentService.GetAll();

            Console.WriteLine("\nAppointment booked successfully.");
            Console.WriteLine($"Assigned Slot: {appointment.Slot}");
            Console.WriteLine(appointment.GetDetails(allAppointments));
        }
        else
        {
            Console.WriteLine("Appointments can only be booked within 6 months from today.");
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (PastDateException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (DoctorUnavailableException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (DoctorNotFoundException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Booking failed: {ex.Message}");
    }
    catch (ArgumentException ex)
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
    try
    {
        Console.Write("Enter Patient ID: ");

        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            throw new FormatException("Invalid patient ID.");
        }

        var patient = patientService.GetPatientById(patientId);

        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }

        var appointments = appointmentService.GetAppointmentsByPatient(patientId);

        if (appointments == null || !appointments.Any())
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
            Console.WriteLine($"Time Slot      : {appointment.Slot}");
            Console.WriteLine($"Status         : {appointment.Status}");
            Console.WriteLine($"Cancellation   : {(string.IsNullOrWhiteSpace(appointment.CancellationReason) ? "N/A" : appointment.CancellationReason)}");
        }

        Console.WriteLine("----------------------------------------");
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (PatientNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while viewing appointments: {ex.Message}");
    }
}
void ConfirmCancelOrCompleteAppointment()
{
    try
    {
        Console.Write("Enter your Appointment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int appointmentId))
        {
            throw new FormatException("Invalid appointment ID.");
        }

        var appointment = appointmentService.GetById(appointmentId);

        if (appointment == null)
        {
            Console.WriteLine($"Appointment with Id {appointmentId} not found.");
            return;
        }

        Console.WriteLine($"We have your Appointment with Id {appointmentId}.");
        Console.WriteLine("Please choose the below option to make changes to the status of your appointment.");
        Console.WriteLine("Press 1 to Cancel your appointment");
        Console.WriteLine("Press 2 to Complete your appointment");

        string action = Console.ReadLine() ?? string.Empty;

        if (action == "1")
        {
            Console.Write("Cancellation reason: ");
            string reason = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Cancellation reason cannot be empty.");
            }

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
            return;
        }

        var allAppointments = appointmentService.GetAll();
        Console.WriteLine(appointment.GetDetails(allAppointments));
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (AppointmentConflictException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Operation failed: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while updating appointment status: {ex.Message}");
    }
}
void AddHealthRecord()
{
    try
    {
        Console.Write("Enter Appointment ID: ");

        if (!int.TryParse(Console.ReadLine(), out int appointmentId))
        {
            throw new FormatException("Invalid appointment ID.");
        }

        var appointment = appointmentService.GetById(appointmentId);

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
        string diagnosis = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(diagnosis))
        {
            throw new ArgumentException("Diagnosis cannot be empty.");
        }

        record.Diagnosis = diagnosis;

        Console.Write("Enter Prescription: ");
        string prescription = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(prescription))
        {
            throw new ArgumentException("Prescription cannot be empty.");
        }

        record.Prescription = prescription;

        Console.Write("Enter Additional Notes: ");
        record.Notes = Console.ReadLine() ?? string.Empty;

        healthRecordService.AddRecord(record);

        Console.WriteLine("Health record added successfully.");
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while adding health record: {ex.Message}");
    }
}
void ViewHealthHistory()
{
    try
    {
        Console.Write("Enter Patient ID: ");

        if (!int.TryParse(Console.ReadLine(), out int patientId))
        {
            throw new FormatException("Invalid patient ID.");
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
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (PatientNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while viewing health history: {ex.Message}");
    }
}
void ViewAllPatients()
{
    try
    {
        var patients = patientService.GetAllPatients();

        if (patients == null || !patients.Any())
        {
            Console.WriteLine("No Patients Found");
            return;
        }

        foreach (var patient in patients)
        {
            Console.WriteLine(patient.GetProfileSummary());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while viewing patients: {ex.Message}");
    }
}
void ViewAllDoctors()
{
    try
    {
        var doctors = doctorService.GetAllDoctors();

        if (doctors == null || !doctors.Any())
        {
            Console.WriteLine("No Doctors Found");
            return;
        }

        foreach (var doctor in doctors)
        {
            Console.WriteLine(doctor.GetProfileSummary());
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while viewing doctors: {ex.Message}");
    }
}
void MakeDoctorActive()
{
    try
    {
        ViewAllDoctors();

        Console.Write("Enter Doctor ID: ");

        if (!int.TryParse(Console.ReadLine(), out int doctorId))
        {
            throw new FormatException("Invalid doctor ID.");
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
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (DoctorNotFoundException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while updating doctor status: {ex.Message}");
    }
}
void Update()
{
    try
    {
        Console.WriteLine("==================Updation===================");
        Console.WriteLine("1. To Update Patient");
        Console.WriteLine("2. To Update Doctor");
        Console.Write("Choose an option: ");

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
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while updating portal: {ex.Message}");
    }

    void UpdatePatient()
    {
        try
        {
            ViewAllPatients();

            Console.WriteLine();
            Console.Write("Enter Patient ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new FormatException("Invalid patient ID.");
            }

            var patient = patientService.GetPatientById(id);

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.WriteLine("Press ENTER to keep existing values");

            Console.Write($"Name ({patient.FullName}): ");
            string name = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!Regex.IsMatch(name, @"^[A-Za-z]+( [A-Za-z]+)*$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
                {
                    throw new ArgumentException("Enter a valid name.");
                }

                patient.FullName = name;
            }

            Console.Write($"Phone ({patient.PhoneNumber}): ");
            string phone = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!Regex.IsMatch(phone, @"^\d{10}$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
                {
                    throw new ArgumentException("Enter a valid phone number.");
                }

                patient.PhoneNumber = phone;
            }

            Console.Write($"Email ({patient.Email}): ");
            string email = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
                {
                    throw new ArgumentException("Enter a valid email id.");
                }

                patient.Email = email;
            }

            Console.WriteLine($@"Current Gender: {patient.Gender}
            Enter your Gender:
            Male
            Female
            Transgender
            Other
            Press ENTER to keep existing");

            string input = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                bool isValid = Enum.TryParse(input, true, out Patient.GenderOptions gender);

                if (isValid)
                {
                    patient.Gender = gender;
                }
                else
                {
                    throw new ArgumentException("Enter valid gender from the list.");
                }
            }

            Console.Write($"DOB ({patient.DateOfBirth:yyyy-MM-dd}): ");
            string dobInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(dobInput))
            {
                if (!DateTime.TryParse(dobInput, out DateTime dob) || dob > DateTime.Today)
                {
                    throw new FormatException("Enter a valid date of birth.");
                }

                patient.DateOfBirth = dob;
            }

            var result = patientService.UpdatePatient(patient);

            Console.WriteLine(result ? "Patient updated successfully." : "Update failed.");
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (PatientNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating patient: {ex.Message}");
        }
    }

    void UpdateDoctor()
    {
        try
        {
            ViewAllDoctors();

            Console.WriteLine();
            Console.Write("Enter Doctor ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                throw new FormatException("Invalid doctor ID.");
            }

            var doctor = doctorService.GetById(id);

            if (doctor == null)
            {
                Console.WriteLine("Doctor not found.");
                return;
            }

            Console.WriteLine("Press ENTER to keep existing values");

            Console.Write($"Name ({doctor.FullName}): ");
            string name = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (!Regex.IsMatch(name, @"^[A-Za-z]+( [A-Za-z]+)*$", RegexOptions.None, TimeSpan.FromMilliseconds(500)))
                {
                    throw new ArgumentException("Enter a valid name.");
                }

                doctor.FullName = name;
            }

            Console.WriteLine($"Current Specialisation: {doctor.Specialisation}");
            Console.WriteLine("Available Specialisations:");

            var specialisations = Enum.GetValues<Doctor.SpecialisationOption>();

            for (int i = 0; i < specialisations.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {specialisations[i]}");
            }

            Console.Write("Specialisation, press ENTER to keep existing: ");
            string specInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(specInput))
            {
                if (int.TryParse(specInput, out int specChoice))
                {
                    if (specChoice >= 1 && specChoice <= specialisations.Length)
                    {
                        doctor.Specialisation = specialisations[specChoice - 1];
                    }
                    else
                    {
                        throw new ArgumentException("Invalid specialisation choice.");
                    }
                }
                else if (Enum.TryParse(specInput, true, out Doctor.SpecialisationOption specEnum))
                {
                    doctor.Specialisation = specEnum;
                }
                else
                {
                    throw new ArgumentException("Invalid specialisation.");
                }
            }

            Console.Write($"Experience ({doctor.YearsOfExperience}): ");
            string expInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(expInput))
            {
                if (!int.TryParse(expInput, out int experience) || experience < 0)
                {
                    throw new FormatException("Enter valid years of experience.");
                }

                doctor.YearsOfExperience = experience;
            }

            Console.Write($"Fee ({doctor.ConsultationFee}): ");
            string feeInput = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(feeInput))
            {
                if (!int.TryParse(feeInput, out int fee) || fee < 0)
                {
                    throw new FormatException("Enter a valid consultation fee.");
                }

                doctor.ConsultationFee = fee;
            }

            var result = doctorService.UpdateDoctor(doctor);

            Console.WriteLine(result ? "Doctor updated successfully." : "Update failed.");
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (DoctorNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while updating doctor: {ex.Message}");
        }
    }
}

public partial class Program
{
}