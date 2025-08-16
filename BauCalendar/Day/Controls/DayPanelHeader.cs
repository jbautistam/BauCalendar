using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BauCalendar.Day;

/// <summary>
///     Cabecera del panel del día
/// </summary>
[TemplatePart(Name = DayPanelHeader.ElementDayHeaderLabel, Type = typeof(TextBlock))]
public sealed class DayPanelHeader : Control
{
    // Constantes privadas
    private const string ElementDayHeaderLabel = "PART_DayHeaderLabel";
    // Variables privadas
    private TextBlock? _dayHeaderLabel;

    static DayPanelHeader()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DayPanelHeader), new FrameworkPropertyMetadata(typeof(DayPanelHeader)));
    }

    /// <summary>
    ///     Obtiene el control propietario
    /// </summary>
    private BindingBase GetOwnerBinding(string propertyName)
    {
        return new Binding(propertyName)
                        {
                            Source = Owner
                        };
    }

    /// <summary>
    ///     Rellena la cabecera
    /// </summary>
    private void PopulateHeader()
    {
        if (_dayHeaderLabel is not null)
        {
            BindingBase binding = GetOwnerBinding("CurrentDate");

                binding.StringFormat = "{0:D}";
                _dayHeaderLabel.SetBinding(TextBlock.TextProperty, binding);
        }
    }

    /// <summary>
    ///     Aplica la plantilla
    /// </summary>
    public override void OnApplyTemplate()
    {
        // Llama al método base
        base.OnApplyTemplate();
        // Obtiene el cuadro de texto de la plantilla
        _dayHeaderLabel = GetTemplateChild(ElementDayHeaderLabel) as TextBlock;
        // Rellena la cabecera
        PopulateHeader();
    }

    /// <summary>
    ///     propietario del control
    /// </summary>
    public DayView? Owner { get; set; }
}
