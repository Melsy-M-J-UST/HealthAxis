using Appntmnt.Data;
using Appntmnt.Exceptions;
using Appntmnt.Models;
using Appntmnt.Service;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static Appntmnt.Helpers.ValidationHelpers;
using static Appntmnt.CommonConstants.AppConstants;

namespace Appntmnt.Functions
{
    [ExcludeFromCodeCoverage]
    public class Function
    {
        private readonly AppDbContext db;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        private readonly IAppointmentService appointmentService;
        private readonly IHealthRecordService healthRecordService;

        public Function(
            AppDbContext db,
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            this.db = db;
            this.patientService = patientService;
            this.doctorService = doctorService;
            this.appointmentService = appointmentService;
            this.healthRecordService = healthRecordService;
        }

        private static string ReadOptionalFullName(string current)
        {
            Console.Write($"Name ({current}): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name)) return current;
            if (!FullNameRegex().IsMatch(name)) throw new ArgumentException("Enter a valid name.");
            return name;
        }

        private static string ReadOptionalPhone(string current)
        {
            Console.Write($"Phone ({current}): ");
            string phone = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(phone)) return current;
            if (!PhoneNumberRegex().IsMatch(phone)) throw new ArgumentException("Enter a valid phone number.");
            return phone;
        }

        private static string? ReadOptionalEmail(string? current)
        {
            Console.Write($"Email ({current}): ");
            string email = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(email)) return current;
            if (!EmailRegex().IsMatch(email)) throw new ArgumentException("Enter a valid email id.");
            return email;
        }

        private static Patient.GenderOptions ReadOptionalGender(Patient.GenderOptions current)
        {
            Console.WriteLine($@"Current Gender: {current}
Enter your Gender:
Male
Female
Transgender
Other
Press ENTER to keep existing");

            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return current;
            if (Enum.TryParse(input, true, out Patient.GenderOptions gender) && Enum.IsDefined<Patient.GenderOptions>(gender)) return gender;
            throw new ArgumentException("Enter valid gender from the list.");
        }

        private static DateTime ReadOptionalDob(DateTime current)
        {
            Console.Write($"DOB ({current:yyyy-MM-dd}): ");
            string dobInput = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(dobInput)) return current;
            if (!DateTime.TryParseExact(dobInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob) || dob > DateTime.Today)
                throw new FormatException("Enter a valid date of birth.");
            return dob;
        }
        private static Doctor.SpecialisationOption ReadOptionalSpecialisation(Doctor.SpecialisationOption current)
        {
            Console.WriteLine($"Current Specialisation: {current}");
            Console.Write("Specialisation, press ENTER to keep existing: ");
            string specInput = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(specInput)) return current;
            return GetSpecialisationFromUser();
        }

        private static int ReadOptionalIntWithinRange(int current, string prompt, int min, int max)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return current;
            if (!int.TryParse(input, out int value) || value < min || value > max) throw new FormatException("Enter valid years of experience.");
            return value;
        }

        private static int ReadOptionalNonNegativeInt(int current, string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(input)) return current;
            if (!int.TryParse(input, out int value) || value < 0) throw new FormatException("Enter a valid consultation fee.");
            return value;
        }

        private static Doctor? ChooseDoctorFromList(List<Doctor> doctors)
        {
            while (true)
            {
                int doctorId = ReadIntWithRetry("\nChoose Doctor ID: ", "Invalid doctor ID. Please try again.");

                var selectedDoctor = doctors.FirstOrDefault(d => d.DoctorId == doctorId);

                if (selectedDoctor == null)
                {
                    Console.WriteLine("Invalid doctor selection. Please choose from the available doctors.");
                    continue;
                }

                if (!selectedDoctor.IsActive)
                {
                    return null;
                }

                return selectedDoctor;
            }
        }

        private static DateTime ReadAppointmentDate()
        {
            while (true)
            {
                Console.Write("Appointment date yyyy-MM-dd: ");

                if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                {
                    Console.WriteLine("Invalid appointment date. Please enter date in yyyy-MM-dd format.");
                    continue;
                }

                if (date < DateTime.Today)
                {
                    Console.WriteLine("Appointment date cannot be in the past.");
                    continue;
                }

                if (date == DateTime.Today)
                {
                    Console.WriteLine("Appointment booking can be done only from tomorrow.");
                    continue;
                }

                if (date >= DateTime.Now.AddMonths(6))
                {
                    Console.WriteLine("Appointments can only be booked within 6 months from today.");
                    continue;
                }

                return date;
            }
        }



        public void RegisterPatient()
        {
            try
            {
                Patient p = new Patient
                {
                    FullName = ReadValidFullName(),
                    DateOfBirth = ReadDateOfBirth("Enter your Date of Birth(YYYY-MM-DD): "),
                    Gender = ReadGender(),
                    PhoneNumber = ReadPhone(),
                    Email = ReadEmailOptional(),
                    InsuranceId = ReadInsuranceOptional(),
                    CreatedDate = DateTime.Now
                };

                p.PatientId = db.GetNextPatientId();
                patientService.RegisterPatient(p);
                Console.WriteLine("Patient registered successfully.");
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

        public void AddDoctor()
        {
            try
            {
                Doctor doctor = new Doctor();

                while (true)
                {
                    Console.Write("Enter Full Name: ");
                    string fullName = Console.ReadLine() ?? string.Empty;

                    if (FullNameRegex().IsMatch(fullName))
                    {
                        doctor.FullName = fullName;
                        break;
                    }

                    Console.WriteLine(NameValid);
                }

                doctor.Specialisation = GetSpecialisationFromUser();

                doctor.YearsOfExperience = ReadIntWithRetry("Enter Years of Experience: ", "Enter a valid years of experience.");
                if (doctor.YearsOfExperience < 0 || doctor.YearsOfExperience > 50)
                {
                    throw new ArgumentException("Enter a valid years of experience.");
                }

                doctor.ConsultationFee = ReadIntWithRetry("Enter Consultation Fee: ", "Enter a valid consultation fee.");
                if (doctor.ConsultationFee < 0)
                {
                    throw new ArgumentException("Enter a valid consultation fee.");
                }

                while (true)
                {
                    Console.Write("Is Active (true/false): ");
                    if (bool.TryParse(Console.ReadLine(), out bool isActive))
                    {
                        doctor.IsActive = isActive;
                        break;
                    }

                    Console.WriteLine("Enter valid active status as true or false.");
                }

                doctor.DoctorId = db.GetNextDoctorId();
                doctorService.AddDoctor(doctor);

                Console.WriteLine("Doctor added successfully!");
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

        public void SearchDoctorsBySpecialisation()
        {
            try
            {
                var specialization = GetSpecialisationFromUser();

                var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

                if (doctors == null || doctors.Count == 0)
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

        private static Doctor.SpecialisationOption GetSpecialisationFromUser()
        {
            while (true)
            {
                Console.WriteLine("Choose Specialisation:");
                var specialisations = Enum.GetValues<Doctor.SpecialisationOption>();

                for (int i = 0; i < specialisations.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {specialisations[i]}");
                }

                Console.Write("Enter Specialisation Name or Number: ");
                string specialisationInput = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(specialisationInput, out int choice) && choice >= 1 && choice <= specialisations.Length)
                {
                    return specialisations[choice - 1];
                }

                if (Enum.TryParse(specialisationInput, true, out Doctor.SpecialisationOption result)
                    && Enum.IsDefined<Doctor.SpecialisationOption>(result))
                {
                    return result;
                }

                Console.WriteLine("Invalid specialisation entered. Please try again.");
            }
        }
        private static int ReadIntWithRetry(string prompt, string invalidMessage)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    return value;
                }

                Console.WriteLine(invalidMessage);
            }
        }

        private static string ReadValidFullName()
        {
            while (true)
            {
                Console.Write("Enter your full name: ");
                string fullName = Console.ReadLine() ?? string.Empty;
                if (FullNameRegex().IsMatch(fullName))
                {
                    return fullName;
                }

                Console.WriteLine(NameValid);
            }
        }

        private static DateTime ReadDateOfBirth(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob)
                    && dob <= DateTime.Today)
                {
                    return dob;
                }

                Console.WriteLine("Enter a valid Date of Birth.");
            }
        }

        private static Patient.GenderOptions ReadGender()
        {
            while (true)
            {
                Console.WriteLine("Enter your Gender:");
                Console.WriteLine("Male");
                Console.WriteLine("Female");
                Console.WriteLine("Transgender");
                Console.WriteLine("Other");
                Console.Write("Kindly please enter one among the four given above: ");

                var input = Console.ReadLine();
                if (Enum.TryParse(input, true, out Patient.GenderOptions gender) && Enum.IsDefined<Patient.GenderOptions>(gender))
                {
                    return gender;
                }

                Console.WriteLine("Enter valid gender from the list.");
            }
        }

        private static string ReadPhone()
        {
            while (true)
            {
                Console.Write("Enter your Phone number: ");
                string phoneNumber = Console.ReadLine() ?? string.Empty;
                if (PhoneNumberRegex().IsMatch(phoneNumber))
                {
                    return phoneNumber;
                }

                Console.WriteLine("Enter a valid phone number.");
            }
        }

        private static string ReadEmailOptional()
        {
            while (true)
            {
                Console.Write("Enter your Mail Id: ");
                string email = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(email))
                {
                    return string.Empty;
                }
                if (EmailRegex().IsMatch(email))
                {
                    return email;
                }
                Console.WriteLine("Enter a valid email id.");
            }
        }

        private static string ReadInsuranceOptional()
        {
            while (true)
            {
                Console.Write("Enter your Insurance ID (optional): ");
                string InsuranceId = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(InsuranceId))
                {
                    return string.Empty;
                }
                else if (InsuranceIdRegex().IsMatch(InsuranceId))
                {
                    return InsuranceId.ToUpperInvariant();
                }
                else
                {
                    Console.WriteLine("Insurance ID must follow format INSXXXX where X are digits.");
                }
            }
        }

        public void BookAppointment()
        {
            try
            {
                int patientId = ReadIntWithRetry(PatientId, "Invalid patient ID. Please try again.");

                var patient = patientService.GetPatientById(patientId);

                if (patient == null)
                {
                    Console.WriteLine(NoPatient);
                    return;
                }

                var specialization = GetSpecialisationFromUser();

                var doctors = doctorService.SearchDoctorBySpecialisation(specialization);

                if (doctors == null || doctors.Count == 0)
                {
                    Console.WriteLine("No doctors found for this specialisation.");
                    return;
                }

                Console.WriteLine("\nAvailable Doctors:");

                foreach (var d in doctors)
                {
                    Console.WriteLine($"ID: {d.DoctorId}, Name: Dr. {d.FullName}, Exp: {d.YearsOfExperience} yrs, Fee: {d.ConsultationFee}");
                }
                var doctor = ChooseDoctorFromList(doctors);
                if (doctor == null)
                {

                    Console.WriteLine("Selected doctor is inactive. Please choose another doctor.");
                    return;
                }

                var date = ReadAppointmentDate();

                var appointment = appointmentService.BookAppointment(patient, doctor, date);
                var allAppointments = appointmentService.GetAllAppointments();

                Console.WriteLine("\nAppointment booked successfully.");
                Console.WriteLine($"Assigned Slot: {appointment.TimeSlot}");
                Console.WriteLine(appointment.GetDetails(allAppointments));
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

        public void ViewAppointmentsForPatient()
        {
            try
            {
                int patientId = ReadIntWithRetry(PatientId, "Invalid patient ID. Please try again.");

                var patient = patientService.GetPatientById(patientId);

                if (patient == null)
                {
                    Console.WriteLine(NoPatient);
                    return;
                }

                var appointments = appointmentService.GetAppointmentsByPatient(patientId);

                if (appointments == null || appointments.Count == 0)
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
            catch (PatientNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while viewing appointments: {ex.Message}");
            }
        }

        public void CancelAppointmentOnly()
        {
            try
            {
                int appointmentId;

                while (true)
                {
                    Console.Write("Enter your Appointment ID: ");

                    if (int.TryParse(Console.ReadLine(), out appointmentId))
                    {
                        break;
                    }

                    Console.WriteLine("Invalid appointment ID. Please try again.");
                }

                var appointment = appointmentService.GetAppointmentById(appointmentId);

                if (appointment == null)
                {
                    Console.WriteLine($"Appointment with Id {appointmentId} not found.");
                    return;
                }

                string reason;

                while (true)
                {
                    Console.Write("Cancellation reason: ");
                    reason = Console.ReadLine() ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(reason))
                    {
                        break;
                    }

                    Console.WriteLine("Cancellation reason cannot be empty. Please try again.");
                }

                appointmentService.CancelAppointment(appointmentId, reason);


            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Operation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while cancelling appointment: {ex.Message}");
            }
        }

        public void ConfirmCancelOrCompleteAppointment()
        {
            try
            {
                Console.Write("Enter your Appointment ID: ");

                if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                {
                    throw new FormatException("Invalid appointment ID.");
                }

                var appointment = appointmentService.GetAppointmentById(appointmentId);

                if (appointment == null)
                {
                    Console.WriteLine($"Appointment with Id {appointmentId} not found.");
                    return;
                }

                Console.WriteLine($"We have your Appointment with Id {appointmentId}.");
                Console.WriteLine("Please choose the below option to make changes to the status of your appointment.");
                Console.WriteLine("Press 1 to Cancel the appointment");
                Console.WriteLine("Press 2 to Complete the appointment");
                Console.WriteLine("Press 3 to Confirm the appointment");

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

                }
                else if (action == "2")
                {

                    appointment.Complete();
                    var completed = appointment.Status == Appointment.StatusOption.Completed;
                    Console.WriteLine(completed ? "Appointment completed." : "");
                }
                else if (action == "3")
                {
                    appointment.Confirm();
                }
                else
                {
                    Console.WriteLine("Invalid action.");
                    return;
                }

                var allAppointments = appointmentService.GetAllAppointments();
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

        public void AddHealthRecord()
        {
            try
            {
                Console.Write("Enter Appointment ID: ");

                if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                {
                    throw new FormatException("Invalid appointment ID.");
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

                record.Appointment = appointment;
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

        public void ViewHealthHistory()
        {
            try
            {
                Console.Write("Enter Patient ID: ");

                if (!int.TryParse(Console.ReadLine(), out int patientId))
                {
                    throw new FormatException("Invalid patient ID.");
                }

                var records = healthRecordService.GetRecordsByPatient(patientId);

                if (records == null || records.Count == 0)
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

        public void UpdatePatient()
        {
            try
            {

                int id = ReadIntWithRetry(PatientId, "Invalid patient ID. Please try again.");

                var patient = patientService.GetPatientById(id);

                if (patient == null)
                {
                    Console.WriteLine(NoPatient);
                    return;
                }

                Console.WriteLine("Press ENTER to keep existing values");

                patient.FullName = ReadOptionalFullName(patient.FullName);
                patient.PhoneNumber = ReadOptionalPhone(patient.PhoneNumber);
                patient.Email = ReadOptionalEmail(patient.Email) ?? patient.Email;
                patient.Gender = ReadOptionalGender(patient.Gender);
                patient.DateOfBirth = ReadOptionalDob(patient.DateOfBirth);

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

        public void UpdateDoctor()
        {
            try
            {


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

                doctor.FullName = ReadOptionalFullName(doctor.FullName);
                doctor.Specialisation = ReadOptionalSpecialisation(doctor.Specialisation);
                doctor.YearsOfExperience = ReadOptionalIntWithinRange(doctor.YearsOfExperience, $"Experience ({doctor.YearsOfExperience}): ", 0, 50);
                doctor.ConsultationFee = ReadOptionalNonNegativeInt(doctor.ConsultationFee, $"Fee ({doctor.ConsultationFee}): ");

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

        public void MakeDoctorActive()
        {
            try
            {

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

        public void Update()
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
        }

        public void ViewPatientById()
        {
            try
            {
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

                Console.WriteLine("\n===== Patient Details =====");
                Console.WriteLine($"ID           : {patient.PatientId}");
                Console.WriteLine($"Name         : {patient.FullName}");
                Console.WriteLine($"DOB          : {patient.DateOfBirth:yyyy-MM-dd}");
                Console.WriteLine($"Gender       : {patient.Gender}");
                Console.WriteLine($"Phone        : {patient.PhoneNumber}");
                Console.WriteLine($"Email        : {patient.Email ?? "N/A"}");
                Console.WriteLine($"Insurance ID : {patient.InsuranceId ?? "N/A"}");
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
                Console.WriteLine($"Error while retrieving patient: {ex.Message}");
            }
        }
    }
}
