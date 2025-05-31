using maui_heater_manager.Domains;

namespace maui_heater_manager.ViewModels;

public partial class SettingViewModel : BaseViewModel
{
    public SettingViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "SettingViewModel";
    }
}
