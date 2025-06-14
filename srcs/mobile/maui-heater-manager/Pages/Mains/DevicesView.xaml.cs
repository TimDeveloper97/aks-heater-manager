using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages.Mains;

public partial class DevicesView : ContentView
{
	public DevicesView(IServiceProvider service)
	{
		InitializeComponent();

        BindingContext = new DeviceViewModel(service);
    }
}