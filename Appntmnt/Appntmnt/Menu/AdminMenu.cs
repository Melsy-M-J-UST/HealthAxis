using Appntmnt.Functions;
using System.Diagnostics.CodeAnalysis;
using Appntmnt.Helpers;
using static Appntmnt.CommonConstants.AppConstants;

namespace Appntmnt.Menu
{
    [ExcludeFromCodeCoverage]
    public class AdminMenu
    {
        private readonly Function functions;

        public AdminMenu(Function functions)
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
                    MenuHelpers.DisplayMenu("Admin Menu",
                        "1. Register Patient",
                        "2. Register Doctor",
                        "3. Search Doctor",
                        "4. Book Appointment",
                        "5. View Appointments",
                        "6. Confirm/Cancel/Complete Appointment",
                        "7. Add Health Record",
                        "8. View Health History",
                        "9. View Patient",
                        "10. Update Portal",
                        "11. Activate/Deactivate Doctor",
                        "12. Back");

                    Console.Write(Option);

                    switch (Console.ReadLine())
                    {
                        case "1":
                            functions.RegisterPatient();
                            break;

                        case "2":
                            functions.AddDoctor();
                            break;

                        case "3":
                            functions.SearchDoctorsBySpecialisation();
                            break;

                        case "4":
                            functions.BookAppointment();
                            break;

                        case "5":
                            functions.ViewAppointmentsForPatient();
                            break;

                        case "6":
                            functions.ConfirmCancelOrCompleteAppointment();
                            break;

                        case "7":
                            functions.AddHealthRecord();
                            break;

                        case "8":
                            functions.ViewHealthHistory();
                            break;

                        case "9":
                            functions.ViewPatientById();
                            break;

                        case "10":
                            functions.Update();
                            break;

                        case "11":
                            functions.MakeDoctorActive();
                            break;

                        case "12":
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
