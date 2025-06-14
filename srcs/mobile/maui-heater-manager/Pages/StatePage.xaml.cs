using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class StatePage : BasePage<StateViewModel>
{
    public StatePage(StateViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}