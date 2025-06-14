using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages.Mains;

public partial class NowsView : ContentView
{
	public NowsView(IServiceProvider services)
	{
		InitializeComponent();

		BindingContext = new NowViewModel(services);
	}
}