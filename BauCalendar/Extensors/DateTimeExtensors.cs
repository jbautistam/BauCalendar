using System.Globalization;

namespace BauCalendar.Extensors;

/// <summary>
///     Extensiones de <see cref="DateTime"/>
/// </summary>
internal static class DateTimeExtensors
{
    /// <summary>
    ///     Obtiene el nombre de mes
    /// </summary>
    public static string ToMonthName(this DateTime dateTime) => ToFirstUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month));

    /// <summary>
    ///     Obtiene el nombre corto del mes
    /// </summary>
    public static string ToShortMonthName(this DateTime date) => ToFirstUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month));

    /// <summary>
    ///     Obtiene el nombre de la semana
    /// </summary>
    public static string ToWeekDayName(this DateTime date) => ToFirstUpper($"{date:ddd}");

    /// <summary>
    ///     Devuelve una cadena con el primer carácter en mayúsculas
    /// </summary>
    private static string ToFirstUpper(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;
        else if (value.Length < 2)
            return value.ToUpper();
        else
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
    }
}
