using System.Windows;
using System.Windows.Controls;

namespace BauCalendar.Month;

/// <summary>
///		Control para mostrar los meses de un año
/// </summary>
public partial class YearView : UserControl
{
	// Propiedades
	public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTime), 
																						 typeof(YearView),
																						 new FrameworkPropertyMetadata(DateTime.Now, 
																													   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																													   OnDateChanged));
	public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(nameof(Rows), typeof(int), 
																						 typeof(YearView),
																						 new FrameworkPropertyMetadata(3, 
																													   FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																													   OnRowsChanged));
	public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof(Columns), typeof(int), 
																							typeof(YearView),
																							new FrameworkPropertyMetadata(4, 
																														  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																														  OnColumnsChanged));

	public YearView()
	{
		InitializeComponent();
		DataContext = this;
	}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	private void Update()
	{
		// Limpia los grids
		grdMonths.Children.Clear();
		// Cambia el título
		lblYear.Content = $"{Date.Year:#,##0}";
		// Añade los meses
		AddMonths();
	}

	/// <summary>
	///		Añade los meses
	/// </summary>
	private void AddMonths()
	{
		for (int month = 1; month <= 12; month++)
		{
			MonthView monthView = new();

				// Asigna las propiedades
				monthView.Margin = new Thickness(5);
				// Asigna la fecha
				monthView.Date = new DateTime(Date.Year, month, 1);
				// Añade el control
				grdMonths.Children.Add(monthView);
		}
	}

	/// <summary>
	///		Actualiza el año
	/// </summary>
	private void UpdateYear(int offset)
	{
		// Cambia el año
		Date = new DateTime(Date.Year + offset, 1, 1);
		// Actualiza el control
		Update();
	}

	/// <summary>
	///		Tratamiento del evento de cambio de fecha
	/// </summary>
	private static void OnDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is YearView view && (DateTime) args.NewValue != (DateTime) args.OldValue)
			view.Date = (DateTime) args.NewValue;
	}

	/// <summary>
	///		Tratamiento del evento de cambio del número de filas
	/// </summary>
	private static void OnRowsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is YearView view && (int) args.NewValue != (int) args.OldValue)
			view.Rows = (int) args.NewValue;
	}

	/// <summary>
	///		Tratamiento del evento de cambio del número de columnas
	/// </summary>
	private static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
	{
		if (sender is YearView view && (int) args.NewValue != (int) args.OldValue)
			view.Columns = (int) args.NewValue;
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

	/// <summary>
	///		Filas mostrada en el control
	/// </summary>
	public int Rows
	{
		get { return (int) GetValue(RowsProperty); }
		set 
		{ 
			SetValue(RowsProperty, value); 
			Update();
		}
	}

	/// <summary>
	///		Colmnas mostrada en el control
	/// </summary>
	public int Columns
	{
		get { return (int) GetValue(ColumnsProperty); }
		set 
		{ 
			SetValue(ColumnsProperty, value); 
			Update();
		}
	}

	private void UserControl_Initialized(object sender, EventArgs e)
	{
		Update();
	}

	private void nextMonth_Click(object sender, RoutedEventArgs e)
	{
		UpdateYear(1);
	}

	private void previousMonth_Click(object sender, RoutedEventArgs e)
	{
		UpdateYear(-1);
	}
}
