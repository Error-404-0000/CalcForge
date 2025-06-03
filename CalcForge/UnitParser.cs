using System.Globalization;
using System.Text.RegularExpressions;

namespace CalcForge;

public static class UnitParser
{
    private static readonly Dictionary<string, double> UnitFactors = new()
    {
        ["mm"] = 1.0,
        ["cm"] = 10.0,
        ["m"] = 1000.0,
        ["in"] = 25.4,
        ["ft"] = 304.8
    };

    private static readonly Regex UnitRegex = new(@"(\d+(?:\.\d+)?)(mm|cm|m|in|ft)\b", RegexOptions.IgnoreCase);

    public static string ConvertUnits(string input)
    {
        return UnitRegex.Replace(input, match =>
        {
            double value = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            string unit = match.Groups[2].Value.ToLowerInvariant();
            if (!UnitFactors.TryGetValue(unit, out double factor))
                return match.Value;
            double converted = value * factor;
            return converted.ToString(CultureInfo.InvariantCulture);
        });
    }
}
