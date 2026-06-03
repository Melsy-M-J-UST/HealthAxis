using Appntmnt.Helpers;
using System.Diagnostics.CodeAnalysis;
using Appntmnt.Functions;
using static Appntmnt.CommonConstants.AppConstants;

namespace Appntmnt.Menu
{
    [ExcludeFromCodeCoverage]
    public class MainMenu
    {
        private readonly Function functions;

        public MainMenu(Function functions)
        {
            this.functions = functions;
        }

        public void Show()
        {
            while (true)
            {
                MenuHelpers.DisplayMenu("HealthAxis Portal",
                    "1. Patient",
                    "2. Doctor",
                    "3. Admin",
                    "4. Exit");

                Console.Write(Option);

                switch (Console.ReadLine())
                {
                    case "1":
                        new PatientMenu(functions).Show();
                        break;

                    case "2":
                        new DoctorMenu(functions).Show();
                        break;

                    case "3":
                        new AdminMenu(functions).Show();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
