using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using BauCalendar.Extensors;

namespace BauCalendar.Month;

/// <summary>
///		Control para mostrar la vista de mes
/// </summary>
public partial class MonthView : UserControl
{
	// Propiedades
	public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), 
																						 typeof(MonthView),
																						 new FrameworkPropertyMetadata(DateTime.Now, 
																													   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																													   OnDateChanged));


	public MonthView()
	{
		InitializeComponent();
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	private void Update()
	{
		// Limpia los grids
		grdDaysOfWeek.Children.Clear();
		grdMonth.Children.Clear();
		// Cambia el título
		lblTitle.Content = Date.ToShortMonthName();
		// Añade los nombres de los días de la semana
		AddDaysOfWeek();
		// Añade los días
		AddDays();
	}

	/// <summary>
	///		Añade los días de la semana
	/// </summary>
	private void AddDaysOfWeek()
	{
		DateTime start = GetMonday(Date);

			// Añade los días de la semana
			for (int index = 0; index < 7; index++)
			{
				Label label = new();

					// Crea la etiqueta
					label.Content = start.AddDays(index).ToWeekDayName();
					label.HorizontalAlignment = HorizontalAlignment.Stretch;
					label.VerticalAlignment = VerticalAlignment.Stretch;
					label.Padding = new Thickness(5);
					label.HorizontalContentAlignment = HorizontalAlignment.Center;
					label.Background = new SolidColorBrush(Colors.Blue);
					label.Foreground = new SolidColorBrush(Colors.White);
					// Añade el nombre del día al grid
					grdDaysOfWeek.Children.Add(label);
			}
	}

	/// <summary>
	///		Añade los días
	/// </summary>
	private void AddDays()
	{
		DateTime start = GetMonday(Date);
		DateTime end = new DateTime(Date.Year, Date.Month, DateTime.DaysInMonth(Date.Year, Date.Month));

			// Busca el último domingo del mes
			while (end.DayOfWeek != DayOfWeek.Sunday)
				end = end.AddDays(1);
			// Muestra los botones
			while (start <= end)
			{
				Button button = new();

					// Asigna las propiedades al botón
					button.Content = start.Day.ToString();
					button.Margin = new Thickness(2);
					button.Style = FindResource("CircularButtonStyle") as Style;
					if (start.Month != Date.Month)
						button.Foreground = new SolidColorBrush(Colors.Gray);
					else
						button.Foreground = new SolidColorBrush(Colors.Black);
					// Añade el botón al grid
					grdMonth.Children.Add(button);
					// Incrementa el día
					start = start.AddDays(1);
			}
	}

	/// <summary>
	///		Obtiene el primer lunes anterior a una fecha
	/// </summary>
	private DateTime GetMonday(DateTime date)
	{
		// Busca el primer lunes anterior a una fecha
		while (date.DayOfWeek != DayOfWeek.Monday)
			date = date.AddDays(-1);
		// Devuelve la fecha
		return date;
	}

	/// <summary>
	///		Tratamiento del evento de cambio de fecha
	/// </summary>
	private static void OnDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is MonthView view && (DateTime) args.NewValue != (DateTime) args.OldValue)
			view.Date = (DateTime) args.NewValue;
	}

	/// <summary>
	///		Fecha mostrada en el control
	/// </summary>
	public DateTime Date
	{
		get { return (DateTime) GetValue(DateProperty); }
		set 
		{ 
			SetValue(DateProperty, value); 
			Update();
		}
	}

	private void UserControl_Initialized(object sender, EventArgs e)
	{
		Update();
	}
}
