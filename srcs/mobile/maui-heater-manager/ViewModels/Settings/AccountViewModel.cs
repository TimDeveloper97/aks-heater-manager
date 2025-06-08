using maui_heater_manager.Domains;

namespace maui_heater_manager.ViewModels.Settings;

public partial class AccountViewModel : BaseViewModel
{
    public AccountViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "AccountViewModel";
    }
}
