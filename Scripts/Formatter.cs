using System.Numerics;
using System.Globalization;

public static class Formatter
{
    public static string FormatValue(BigInteger value)
    {
        // ≈сли число меньше 1 млн, выводим как обычно
        if (value < 1000000)
            return value.ToString("N0", CultureInfo.InvariantCulture);

        string str = value.ToString();
        int exponent = str.Length - 1;

        string mantissa = str.Substring(0, 1) + "." + str.Substring(1, 2);

        return $"{mantissa}e{exponent}";
    }
}
