using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BauCalendar.Day;

[TemplatePart(Name = ElementLedgerItems, Type = typeof(StackPanel))]
public sealed class DayLedger : Control
{
    // Constantes privadas
    private const string ElementLedgerItems = "PART_LedgerItems";
    // Variables privadas
    private StackPanel? _ledgerItems;
    // Propiedades de dependencia
    public static readonly DependencyProperty CalendarLedgerItemStyleProperty = DayView.CalendarLedgerItemStyleProperty.AddOwner(typeof(DayLedger));

    static DayLedger()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayLedger), new FrameworkPropertyMetadata(typeof(DayLedger)));
    }

    /// <summary>
    ///     Rellena los elementos
    /// </summary>
    private void PopulateLedger()
    {
        if (_ledgerItems is not null)
            for (int index = 0; index < 24; index++)
            {
                DayLedgerItem item = new();

                    // Asigna las propiedades
                    item.TimeslotA = index.ToString();
                    item.TimeslotB = "00";
                    item.SetBinding(DayLedgerItem.StyleProperty, GetOwnerBinding("CalendarLedgerItemStyle"));
                    // Añade el elemento a la lista
                    _ledgerItems.Children.Add(item);
            }
    }

    /// <summary>
    ///     Obtiene el propietario del control
    /// </summary>
    private BindingBase GetOwnerBinding(string propertyName)
    {
        return new Binding(propertyName)
                        {
                            Source = Owner
                        };
    }

    /// <summary>
    ///     Sobrescribe el método de aplicar la plantilla
    /// </summary>
    public override void OnApplyTemplate()
    {
        // Llama al método base
        base.OnApplyTemplate();
        // Obtiene los elementos
        _ledgerItems = GetTemplateChild(ElementLedgerItems) as StackPanel;
        // Rellena los elementos
        PopulateLedger();
    }

    /// <summary>
    ///     Propietario del control
    /// </summary>
    public DayView Owner { get; set; } = default!;

    /// <summary>
    ///     Estilo
    /// </summary>
    public Style CalendarLedgerItemStyle
    {
        get { return (Style) GetValue(CalendarLedgerItemStyleProperty); }
        set { SetValue(CalendarLedgerItemStyleProperty, value); }
    }
}
