using Appntmnt.Functions;
using Appntmnt.Helpers;
using System.Diagnostics.CodeAnalysis;
using static Appntmnt.CommonConstants.AppConstants;


namespace Appntmnt.Menu
{
    [ExcludeFromCodeCoverage]
    public class DoctorMenu
    {
        private readonly Function functions;

        public DoctorMenu(Function functions)
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
                    MenuHelpers.DisplayMenu("Doctor Menu",
                        "1. Register Doctor",
                        "2. View Appointments",
                        "3. Confirm/Cancel/Complete Appointment",
                        "4. Add Health Record",
                        "5. View Health History",
                        "6. View Patient",
                        "7. Update Doctor",
                        "8. Back");

                    Console.Write(Option);

                    switch (Console.ReadLine())
                    {
                        case "1":
                            functions.AddDoctor();
                            break;

                        case "2":
                            functions.ViewAppointmentsForPatient();
                            break;

                        case "3":
                            functions.ConfirmCancelOrCompleteAppointment();
                            break;

                        case "4":
                            functions.AddHealthRecord();
                            break;

                        case "5":
                            functions.ViewHealthHistory();
                            break;

                        case "6":
                            functions.ViewPatientById();
                            break;

                        case "7":
                            functions.UpdateDoctor();
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
