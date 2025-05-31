using CommunityToolkit.Mvvm.ComponentModel;

namespace maui_heater_manager.Domains;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy = false;

    [ObservableProperty]
    private string title = string.Empty;
}
