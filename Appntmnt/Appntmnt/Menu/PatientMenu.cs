using System.Diagnostics.CodeAnalysis;
using Appntmnt.Functions;
using Appntmnt.Helpers;
using static Appntmnt.CommonConstants.AppConstants;

namespace Appntmnt.Menu
{
    [ExcludeFromCodeCoverage]
    public class PatientMenu
    {
        private readonly Function functions;

        public PatientMenu(Function functions)
        {
            this.functions = functions;
        }

        public void Show()
        {
            bool exit = false;

            while (!exit)
            {
                try
                {
                    MenuHelpers.DisplayMenu("Patient Menu",
                        "1. Register Patient",
                        "2. Search Doctor by Specialisation",
                        "3. Book Appointment",
                        "4. View Appointments",
                        "5. Cancel Appointment",
                        "6. View Health History",
                        "7. Update Patient",
                        "8. Back");

                    Console.Write(Option);

                    switch (Console.ReadLine())
                    {
                        case "1":
                            functions.RegisterPatient();
                            break;

                        case "2":
                            functions.SearchDoctorsBySpecialisation();
                            break;

                        case "3":
                            functions.BookAppointment();
                            break;

                        case "4":
                            functions.ViewAppointmentsForPatient();
                            break;

                        case "5":
                            functions.CancelAppointmentOnly();
                            break;

                        case "6":
                            functions.ViewHealthHistory();
                            break;

                        case "7":
                            functions.UpdatePatient();
                            break;

                        case "8":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
