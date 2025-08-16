using System.Windows;
using System.Windows.Controls;

namespace BauCalendar.Day;

/// <summary>
///     Elemento del día
/// </summary>
public sealed class DayLedgerItem : Control
{
    // Propiedades de dependencia
    public static readonly DependencyProperty TimeslotAProperty = DependencyProperty.Register(nameof(TimeslotA), typeof(string), typeof(DayLedgerItem),
                                                                                              new FrameworkPropertyMetadata(string.Empty));
    public static readonly DependencyProperty TimeslotBProperty = DependencyProperty.Register(nameof(TimeslotB), typeof(string), typeof(DayLedgerItem),
                                                                                              new FrameworkPropertyMetadata(string.Empty));
    static DayLedgerItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayLedgerItem), new FrameworkPropertyMetadata(typeof(DayLedgerItem)));
    }

    /// <summary>
    ///     Inicio del periodo
    /// </summary>
    public string TimeslotA
    {
        get { return (string) GetValue(TimeslotAProperty); }
        set { SetValue(TimeslotAProperty, value); }
    }

    /// <summary>
    ///     Fin del periodo
    /// </summary>
    public string TimeslotB
    {
        get { return (string) GetValue(TimeslotBProperty); }
        set { SetValue(TimeslotBProperty, value); }
    }
}
