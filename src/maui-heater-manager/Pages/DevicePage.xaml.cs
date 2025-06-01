using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class DevicePage : BasePage<DeviceViewModel>
{
    public DevicePage(DeviceViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}