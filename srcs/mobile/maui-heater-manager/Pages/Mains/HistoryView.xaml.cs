using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages.Mains;

public partial class HistoryView : ContentView
{
	public HistoryView(IServiceProvider service)
	{
		InitializeComponent();

        BindingContext = new UsageViewModel(service);
    }
}