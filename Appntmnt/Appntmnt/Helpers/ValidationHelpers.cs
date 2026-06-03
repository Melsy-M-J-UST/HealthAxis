using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Appntmnt.Helpers
{
    [ExcludeFromCodeCoverage]
    public static partial class ValidationHelpers
    {
        [GeneratedRegex(@"^[A-Za-z]+( [A-Za-z]+)*$", RegexOptions.CultureInvariant)]
        public static partial Regex FullNameRegex();

        [GeneratedRegex(@"^\d{10}$", RegexOptions.CultureInvariant)]
        public static partial Regex PhoneNumberRegex();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.CultureInvariant)]
        public static partial Regex EmailRegex();

        [GeneratedRegex(@"^INS\d{4}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
        public static partial Regex InsuranceIdRegex();
    }
}
