using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages.Mains;

public partial class SettingView : ContentView
{
	public SettingView(IServiceProvider service)
	{
		InitializeComponent();

        BindingContext = new SettingViewModel(service);
    }
}