using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class UsagePage : BasePage<UsageViewModel>
{
    public UsagePage(UsageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}