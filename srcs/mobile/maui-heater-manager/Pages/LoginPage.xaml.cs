using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class LoginPage : BasePage<LoginViewModel>
{
    public LoginPage(LoginViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}