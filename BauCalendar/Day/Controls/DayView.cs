using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using RudiGrobler.Calendar.Common;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BauCalendar.Day;

[TemplatePart(Name = DayView.ElementDay, Type = typeof(DayPanel))]
[TemplatePart(Name = DayView.ElementDayHeader, Type = typeof(DayPanelHeader))]
[TemplatePart(Name = DayView.ElementLedger, Type = typeof(DayLedger))]
[TemplatePart(Name = DayView.ElementScrollViewer, Type = typeof(ScrollViewer))]
public partial class DayView : Control
{
    private const string ElementDay = "PART_Day";
    private const string ElementDayHeader = "PART_DayHeader";
    private const string ElementLedger = "PART_Ledger";
    private const string ElementScrollViewer = "PART_ScrollViewer";

    static DayView()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayView), new FrameworkPropertyMetadata(typeof(DayView)));

        CommandManager.RegisterClassCommandBinding(typeof(DayView), new CommandBinding(NextDay, new ExecutedRoutedEventHandler(OnExecutedNextDay), new CanExecuteRoutedEventHandler(OnCanExecuteNextDay)));
        CommandManager.RegisterClassCommandBinding(typeof(DayView), new CommandBinding(PreviousDay, new ExecutedRoutedEventHandler(OnExecutedPreviousDay), new CanExecuteRoutedEventHandler(OnCanExecutePreviousDay)));
    }

    public static readonly DependencyProperty CalendarTimeslotItemStyleProperty =
        DependencyProperty.Register("CalendarTimeslotItemStyle", typeof(Style), typeof(DayView));

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public Style CalendarTimeslotItemStyle
    {
        get { return (Style)GetValue(CalendarTimeslotItemStyleProperty); }
        set { SetValue(CalendarTimeslotItemStyleProperty, value); }
    }

    public static readonly DependencyProperty CalendarLedgerItemStyleProperty =
        DependencyProperty.Register("CalendarLedgerItemStyle", typeof(Style), typeof(DayView));

    [Category("Calendar")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public Style CalendarLedgerItemStyle
    {
        get { return (Style)GetValue(CalendarLedgerItemStyleProperty); }
        set { SetValue(CalendarLedgerItemStyleProperty, value); }
    }

    public static readonly DependencyProperty CalendarAppointmentItemStyleProperty =
        DependencyProperty.Register("CalendarAppointmentItemStyle", typeof(Style), typeof(DayView));

    [Category("Calendar")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public Style CalendarAppointmentItemStyle
    {
        get { return (Style)GetValue(CalendarAppointmentItemStyleProperty); }
        set { SetValue(CalendarAppointmentItemStyleProperty, value); }
    }

    public static readonly RoutedEvent AddAppointmentEvent = 
        TimeSlotItem.AddAppointmentEvent.AddOwner(typeof(DayPanel));

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

    public static readonly DependencyProperty AppointmentsProperty =
        DependencyProperty.Register("Appointments", typeof(IEnumerable<Appointment>), typeof(DayView),
        new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DayView.OnAppointmentsChanged)));

    public IEnumerable<Appointment> Appointments
    {
        get { return (ObservableCollection<Appointment>)GetValue(AppointmentsProperty); }
        set { SetValue(AppointmentsProperty, value); }
    }

    private static void OnAppointmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((DayView)d).OnAppointmentsChanged(e);
    }

    protected virtual void OnAppointmentsChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_day != null)
        {
            _day.PopulateDay();
        }

        INotifyCollectionChanged appointments = Appointments as INotifyCollectionChanged;
        if (appointments != null)
        {
            appointments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Appointments_CollectionChanged);
        }
        FilterAppointments(CurrentDate);
    }

    void Appointments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        FilterAppointments(CurrentDate);
    }

    public static readonly DependencyProperty CurrentDateProperty =
        DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(DayView),
            new FrameworkPropertyMetadata((DateTime)DateTime.Now, FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnCurrentDateChanged)));

    [Category("Calendar")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public DateTime CurrentDate
    {
        get { return (DateTime)GetValue(CurrentDateProperty); }
        set { SetValue(CurrentDateProperty, value); }
    }

    private static void OnCurrentDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((DayView)d).OnCurrentDateChanged(e);
    }

    protected virtual void OnCurrentDateChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_day != null)
        {
            _day.PopulateDay();
        }

        FilterAppointments(CurrentDate);
    }

    public static readonly DependencyProperty PeakTimeslotBackgroundProperty =
        DependencyProperty.Register("PeakTimeslotBackground", typeof(Brush), typeof(DayView),
            new FrameworkPropertyMetadata((Brush)Brushes.White,
                new PropertyChangedCallback(OnPeakTimeslotBackgroundChanged)));

    [Category("Calendar")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public Brush PeakTimeslotBackground
    {
        get { return (Brush)GetValue(PeakTimeslotBackgroundProperty); }
        set { SetValue(PeakTimeslotBackgroundProperty, value); }
    }

    private static void OnPeakTimeslotBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((DayView)d).OnPeakTimeslotBackgroundChanged(e);
    }

    protected virtual void OnPeakTimeslotBackgroundChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_day != null)
        {
            _day.PopulateDay();
        }
    }

    public static readonly DependencyProperty OffPeakTimeslotBackgroundProperty =
        DependencyProperty.Register("OffPeakTimeslotBackground", typeof(Brush), typeof(DayView),
            new FrameworkPropertyMetadata((Brush)Brushes.LightCyan,
                new PropertyChangedCallback(OnOffPeakTimeslotBackgroundChanged)));

    [Category("Calendar")]
    [EditorBrowsable(EditorBrowsableState.Always)]
    public Brush OffPeakTimeslotBackground
    {
        get { return (Brush)GetValue(OffPeakTimeslotBackgroundProperty); }
        set { SetValue(OffPeakTimeslotBackgroundProperty, value); }
    }

    private static void OnOffPeakTimeslotBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((DayView)d).OnOffPeakTimeslotBackgroundChanged(e);
    }

    protected virtual void OnOffPeakTimeslotBackgroundChanged(DependencyPropertyChangedEventArgs e)
    {
        if (_day != null)
        {
            _day.PopulateDay();
        }
    }

    private void FilterAppointments(DateTime date)
    {            
        if (_day != null)
        {
            _day.ItemsSource = Appointments.ByDate(date);
        }
    }

    
    DayLedger _ledger;
    DayPanelHeader _dayHeader;
    ScrollViewer _scrollViewer;

DayPanel _day;


    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _ledger = GetTemplateChild(ElementLedger) as DayLedger;
        if (_ledger != null)
        {
            _ledger.Owner = this;
        }

        _day = GetTemplateChild(ElementDay) as DayPanel;
        if (_day != null)
        {
            _day.Owner = this;
        }

        _dayHeader = GetTemplateChild(ElementDayHeader) as DayPanelHeader;
        if (_dayHeader != null)
        {
            _dayHeader.Owner = this;
        }

        _scrollViewer = GetTemplateChild(ElementScrollViewer) as ScrollViewer;
    }

    public void ScrollToHome()
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollToHome();
        }
    }

    public void ScrollToOffset(double offset)
    {
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollToHorizontalOffset(offset);
        }
    }

    public static readonly RoutedCommand NextDay = new RoutedCommand("NextDay", typeof(DayView));
    public static readonly RoutedCommand PreviousDay = new RoutedCommand("PreviousDay", typeof(DayView));

    private static void OnCanExecuteNextDay(object sender, CanExecuteRoutedEventArgs e)
    {
        ((DayView)sender).OnCanExecuteNextDay(e);
    }

    private static void OnExecutedNextDay(object sender, ExecutedRoutedEventArgs e)
    {
        ((DayView)sender).OnExecutedNextDay(e);
    }

    protected virtual void OnCanExecuteNextDay(CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        e.Handled = false;
    }

    protected virtual void OnExecutedNextDay(ExecutedRoutedEventArgs e)
    {
        CurrentDate += TimeSpan.FromDays(1);
        e.Handled = true;            
    }

    private static void OnCanExecutePreviousDay(object sender, CanExecuteRoutedEventArgs e)
    {
        ((DayView)sender).OnCanExecutePreviousDay(e);
    }

    private static void OnExecutedPreviousDay(object sender, ExecutedRoutedEventArgs e)
    {
        ((DayView)sender).OnExecutedPreviousDay(e);
    }

    protected virtual void OnCanExecutePreviousDay(CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
        e.Handled = false;
    }

    protected virtual void OnExecutedPreviousDay(ExecutedRoutedEventArgs e)
    {
        CurrentDate -= TimeSpan.FromDays(1);
        e.Handled = true;
    }
}