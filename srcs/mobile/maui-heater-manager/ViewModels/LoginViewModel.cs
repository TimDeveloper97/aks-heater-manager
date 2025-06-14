using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using maui_heater_manager.Pages;

namespace maui_heater_manager.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    public LoginViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "Login";
    }


    [RelayCommand]
    async Task Login() => await Shell.Current.GoToAsync(nameof(StatePage));
}
