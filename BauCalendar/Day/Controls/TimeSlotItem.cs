using System.Windows;
using System.Windows.Controls.Primitives;

namespace BauCalendar.Day;

public partial class TimeSlotItem : ButtonBase
{
    public const string StateNormal = "Normal";
    public const string StateMouseOver = "MouseOver";
    public const string StateDisabled = "Disabled";

    public const string GroupCommon = "CommonStates";

    static TimeSlotItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TimeSlotItem), new FrameworkPropertyMetadata(typeof(TimeSlotItem)));
    }

    private void RaiseAddAppointmentEvent()
    {
        RoutedEventArgs e = new RoutedEventArgs();
        e.RoutedEvent = AddAppointmentEvent;
        e.Source = this;
        
        OnAddAppointment(e);
    }

    public static readonly RoutedEvent AddAppointmentEvent = 
        EventManager.RegisterRoutedEvent("AddAppointment", RoutingStrategy.Bubble,
        typeof(RoutedEventArgs), typeof(TimeSlotItem));

    public event RoutedEventHandler AddAppointment
    {
        add
        {
            AddHandler(AddAppointmentEvent, value);
        }
        remove
        {
            RemoveHandler(AddAppointmentEvent, value);
        }
    }

    protected virtual void OnAddAppointment(RoutedEventArgs e)
    {
        RaiseEvent(e);
    }

    protected override void OnClick()
    {
        base.OnClick();

        RaiseAddAppointmentEvent();
    }

    /// <summary>
    /// StartTime Dependency Property
    /// </summary>
    public static readonly DependencyProperty StartTimeProperty =
        DependencyProperty.Register("StartTime", typeof(DateTime), typeof(TimeSlotItem),
            new FrameworkPropertyMetadata((DateTime)DateTime.Now));

    /// <summary>
    /// Gets or sets the StartTime property.  This dependency property 
    /// indicates ....
    /// </summary>
    public DateTime StartTime
    {
        get { return (DateTime)GetValue(StartTimeProperty); }
        set { SetValue(StartTimeProperty, value); }
    }

    /// <summary>
    /// StartTime Dependency Property
    /// </summary>
    public static readonly DependencyProperty EndTimeProperty =
        DependencyProperty.Register("EndTime", typeof(DateTime), typeof(TimeSlotItem),
            new FrameworkPropertyMetadata((DateTime)DateTime.Now));

    /// <summary>
    /// Gets or sets the StartTime property.  This dependency property 
    /// indicates ....
    /// </summary>
    public DateTime EndTime
    {
        get { return (DateTime)GetValue(EndTimeProperty); }
        set { SetValue(EndTimeProperty, value); }
    }
}