using maui_heater_manager.Domains;

namespace maui_heater_manager.ViewModels;

public partial class UsageViewModel : BaseViewModel
{
    public UsageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "UsageViewModel";
    }
}
