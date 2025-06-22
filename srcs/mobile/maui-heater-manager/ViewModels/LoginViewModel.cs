using AquilaService.Interfaces;
using AquilaService.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using maui_heater_manager.Pages;
using System.Windows.Input;
using VstCommon;
using VstCommon.ModelResponses;
using VstCommon.Models;

namespace maui_heater_manager.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IRestApiService _restApiService;

    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    public LoginViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _restApiService = serviceProvider.GetService<IRestApiService>() ?? throw new Exception("");

        Title = "Login";
    }

    [RelayCommand]
    void PageAppearing()
    {
        Username = "0394852798";
        Password = "2798";
    }

    [RelayCommand]
    async Task Login()
    {
        var rdata = new RData
        {
            Data = new VstRequest { Value = new LoginRequest { Password = Password, Username = Username } },
            Endpoint = API.Login,
        };

        await _restApiService.VstRequestAPI<LoginResponse>(
            ERMethod.Post,
            rdata,
            onSuccess: async (response) =>
            {
                API.UserType = response.Model?.Role?.ToLower() ?? API.CUSTOMER;
                _user = response.Model;

                await Shell.Current.GoToAsync(nameof(StatePage));
            },
            onFailure: async (e) =>
            {
                await Shell.Current.DisplayAlert("Information", "Login fail", "Cancel");
            });
    }
}
