using maui_heater_manager.Domains;
using maui_heater_manager.ViewModels;

namespace maui_heater_manager.Pages;

public partial class NowPage : BasePage<NowViewModel>
{
	public NowPage(NowViewModel viewModel) : base(viewModel)
    {
		InitializeComponent();
	}
}