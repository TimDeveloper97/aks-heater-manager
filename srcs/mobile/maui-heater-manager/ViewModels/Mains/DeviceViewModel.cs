using CommunityToolkit.Mvvm.ComponentModel;
using maui_heater_manager.Domains;
using System.Collections.ObjectModel;
using maui_heater_manager.Models.Devices;
using Device = maui_heater_manager.Models.Devices.Device;

namespace maui_heater_manager.ViewModels;

public partial class DeviceViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<Device> devices = new();

    public DeviceViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "DeviceViewModel";

        GenerateRandomDevices();
    }
}

public partial class DeviceViewModel
{
    private void GenerateRandomDevices()
    {
        Devices.Add(new Device
        {
            Title = "AC 30",
            Description = "316 w",
            Icon = "fan.png",
            Color = Colors.LightBlue
        });
        Devices.Add(new Device
        {
            Title = "Always On",
            Description = "291 w",
            Icon = "home.png",
            Color = Colors.AliceBlue
        });
        Devices.Add(new Device
        {
            Title = "Heat Pump Water Heater 1",
            Description = "206 w",
            Icon = "setting.png",
            Color = Colors.CadetBlue
        });

        for (int i = 0; i < 29; i++)
        {
            Devices.Add(new Device
            {
                Title = $"AC {i + 1}",
                Description = "Off",
                Icon = "fan.png",
                Color = Colors.Gray
            });
        }
    }
}
