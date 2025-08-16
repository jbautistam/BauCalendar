using System.Windows;
using System.Windows.Controls;

namespace BauCalendar.Day;

/// <summary>
///     Control para representar una cita en el calendario
/// </summary>
public sealed class DayAppointmentItem : ContentControl
{
    // Constantes públicas
    public const string StateNormal = "Normal";
    public const string StateMouseOver = "MouseOver";
    public const string StateDisabled = "Disabled";
    public const string GroupCommon = "CommonStates";
    // Propiedades
    public static readonly DependencyProperty StartTimeProperty = TimeSlotPanel.StartTimeProperty.AddOwner(typeof(DayAppointmentItem));
    public static readonly DependencyProperty EndTimeProperty = TimeSlotPanel.EndTimeProperty.AddOwner(typeof(DayAppointmentItem));

    static DayAppointmentItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayAppointmentItem), new FrameworkPropertyMetadata(typeof(DayAppointmentItem)));
    }

    /// <summary>
    ///     Indica si es la hora de inicio
    /// </summary>
    public bool StartTime
    {
        get { return (bool) GetValue(StartTimeProperty); }
        set { SetValue(StartTimeProperty, value); }
    }

    /// <summary>
    ///     Indica si es la hora de fin
    /// </summary>
    public bool EndTime
    {
        get { return (bool) GetValue(EndTimeProperty); }
        set { SetValue(EndTimeProperty, value); }
    }
}
