using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels.Settings;

namespace maui_heater_manager.Pages.Settings;

public partial class AccountPage : BasePage<AccountViewModel>
{
    public AccountPage(AccountViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}