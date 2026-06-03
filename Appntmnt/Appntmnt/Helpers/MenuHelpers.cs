using System.Diagnostics.CodeAnalysis;

namespace Appntmnt.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class MenuHelpers
    {
        public static void DisplayMenu(string title, params string[] options)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {title} =====");
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }
        }
    }
}
