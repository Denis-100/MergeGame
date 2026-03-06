using System.Numerics;
using System.Globalization;

public static class Formatter
{
    public static string FormatValue(BigInteger value)
    {
        // Если число меньше 10 млн, выводим как обычно
        if (value < 10000000)
            return value.ToString("N0", CultureInfo.InvariantCulture);

        // Формат "E2" дает вид 1.23E+015. 
        // Заменяем "E+" на "e" для более компактного вида "1.23e15"
        double d = (double)value;
        return d.ToString("E2", CultureInfo.InvariantCulture).Replace("E+", "e");
    }
}
