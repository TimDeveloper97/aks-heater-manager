using AquilaService.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using System.Collections.ObjectModel;
using VstCommon.ModelResponses;
using VstCommon.Models;
using VstCommon;
using AquilaService.Interfaces;

namespace maui_heater_manager.ViewModels;

public partial class StateViewModel : BaseViewModel
{
    private readonly IRestApiService _restApiService;

    [ObservableProperty]
    ObservableCollection<Models.Devices.Device> states = new();

    public StateViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _restApiService = serviceProvider.GetService<IRestApiService>() ?? throw new Exception("");

        Title = "StateViewModel";
    }

    [RelayCommand]
    async Task PageAppearing()
    {
        var rdata = new RData
        {
            Data = new VstRequest { Token = _user?.Token },
            Endpoint = API.DeviceList,
        };

        await _restApiService.VstRequestAPI<List<DeviceResponse>>(
            ERMethod.Post,
            rdata,
            onSuccess: async (response) => GenerateStates(response.Model),
            onFailure: async (e) =>
            {
                await Shell.Current.DisplayAlert("Information", "Cann't get information device", "Cancel");
            });
    }

    [RelayCommand]
    async Task Navigate()
    {
        await Shell.Current.GoToAsync($"MainPage");
    }
}

public partial class StateViewModel
{
    private void GenerateStates(List<DeviceResponse>? deviceResponses)
    {
        if (deviceResponses is null
            || deviceResponses.Count == 0)
            return;

        foreach (var dr in deviceResponses)
        {
            States.Add(new Models.Devices.Device
            {
                Title = dr.Name,
                Description = dr.Addr,
                Version = dr.Version,
                Icon = "device_v2.png",
            });
        }
    }
}

