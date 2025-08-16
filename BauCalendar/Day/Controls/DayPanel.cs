using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BauCalendar.Day;

/// <summary>
///     Panel con los elementos del día
/// </summary>
[TemplatePart(Name = ElementTimeslotItems, Type = typeof(StackPanel))]
public sealed class DayPanel : ItemsControl
{
    // Constantes privadas
    private const string ElementTimeslotItems = "PART_TimeslotItems";
    // Variables privadas
    private StackPanel? _dayItems;

    static DayPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayPanel), new FrameworkPropertyMetadata(typeof(DayPanel)));
    }

    /// <summary>
    ///     Rellena los datos del día
    /// </summary>
    public void PopulateDay()
    {
        if (_dayItems is not null)
        {
            DateTime startTime = Owner?.CurrentDate.Date ?? DateTime.Now.Date;

                _dayItems.Children.Clear();
                for (int index = 0; index < 48; index++)
                {                   
                    TimeSlotItem timeslot = new();

                    timeslot.StartTime = startTime;
                    timeslot.EndTime = startTime + TimeSpan.FromMinutes(30);

                    if (startTime.Hour >= 8 && startTime.Hour <= 17)
                        timeslot.SetBinding(DayView.BackgroundProperty, GetOwnerBinding("PeakTimeslotBackground"));
                    else
                        timeslot.SetBinding(DayView.BackgroundProperty, GetOwnerBinding("OffPeakTimeslotBackground"));

                    timeslot.SetBinding(TimeSlotItem.StyleProperty, GetOwnerBinding("CalendarTimeslotItemStyle"));
                    _dayItems.Children.Add(timeslot);

                    startTime = startTime + TimeSpan.FromMinutes(30);
                }
        }
        // Cambia el scroll al primer elemento
        if (Owner is not null)
            Owner.ScrollToHome();
    }

    /// <summary>
    ///     Aplica la plantilla
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _dayItems = GetTemplateChild(ElementTimeslotItems) as StackPanel;

        PopulateDay();
    }

    /// <summary>
    ///     Obtiene el propitario
    /// </summary>
    private BindingBase GetOwnerBinding(string propertyName)
    {
        return new Binding(propertyName)
                        {
                            Source = Owner
                        };
    }

    /// <summary>
    ///     Obtiene el contenedor
    /// </summary>
    protected override DependencyObject GetContainerForItemOverride() => new DayAppointmentItem();

    /// <summary>
    ///     Indica si los elementos están en un contenedor
    /// </summary>
    protected override bool IsItemItsOwnContainerOverride(object item) => item is DayAppointmentItem;

    /// <summary>
    ///     Propietario del control
    /// </summary>
    public DayView? Owner { get; set; }
}
