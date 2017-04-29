using System.Globalization;

namespace GeneticProgramming.Data.Reporting
{
    public static class DoubleExtensions
    {
        private static CultureInfo _gnCulture=CultureInfo.GetCultureInfo("en-GB");

        public static string ToGBString(this double value)
        {
            return value.ToString(_gnCulture);
        }
    }
}