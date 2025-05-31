using maui_heater_manager.Domains;

namespace maui_heater_manager.ViewModels;

public partial  class DeviceViewModel : BaseViewModel
{
    public DeviceViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "DeviceViewModel";
    }
}
