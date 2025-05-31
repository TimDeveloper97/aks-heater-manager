using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Windows.Services.Maps;

namespace maui_heater_manager.Domains;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string title = string.Empty;
}

public abstract partial class BaseViewModel
{
    private readonly ILogger<BaseViewModel> _logger;

    public BaseViewModel(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetService<ILogger<BaseViewModel>>()
            ?? throw new Exception("");
    }
}
