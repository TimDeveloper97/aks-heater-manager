using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class SettingPage : BasePage<SettingViewModel>
{
    public SettingPage(SettingViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}