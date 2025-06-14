using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class MainPage : BasePage<MainViewModel>
{
    public MainPage(MainViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}