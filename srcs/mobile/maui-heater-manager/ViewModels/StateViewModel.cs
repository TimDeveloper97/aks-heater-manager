using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using System.Collections.ObjectModel;

namespace maui_heater_manager.ViewModels;

public partial class StateViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<Models.Devices.Device> states = new();

    public StateViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "StateViewModel";

        GenerateRandomStates();
    }

    [RelayCommand]
    async Task Navigate()
    {
        await Shell.Current.GoToAsync($"MainPage");
    }
}

public partial class StateViewModel
{
    private void GenerateRandomStates()
    {
        States.Add(new Models.Devices.Device
        {
            Title = "AC 30",
            Status = "316 w",
            Icon = "fan.png",
            Color = Colors.LightBlue
        });
        States.Add(new Models.Devices.Device
        {
            Title = "Always On",
            Status = "291 w",
            Icon = "home.png",
            Color = Colors.AliceBlue
        });
        States.Add(new Models.Devices.Device
        {
            Title = "Heat Pump Water Heater 1",
            Status = "206 w",
            Icon = "setting.png",
            Color = Colors.CadetBlue
        });
    }
}

