using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using maui_heater_manager.Models.Settings;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace maui_heater_manager.ViewModels;

public partial class SettingViewModel : BaseViewModel
{
    [ObservableProperty]
    ObservableCollection<SettingItem> settingItems = new();

    [ObservableProperty]
    ObservableCollection<SettingItem> additionalItems = new();

    public SettingViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "SettingViewModel";

        GenerateRandomSettings();
    }

    [RelayCommand]
    async Task Navigate(SettingItem setting)
    {
        await Shell.Current.GoToAsync($"{setting.Title}Page");
    }
}

public partial class SettingViewModel
{
    private void GenerateRandomSettings()
    {
        SettingItems.Add(new SettingItem
        {
            Title = "Account",
            Icon = "account_color.png",
        });
        SettingItems.Add(new SettingItem
        {
            Title = "My Home",
            Icon = "home_color.png",
        });
        SettingItems.Add(new SettingItem
        {
            Title = "General",
            Icon = "setting_color.png",
        });
        AdditionalItems.Add(new SettingItem
        {
            Title = "Help",
            Icon = "help_color.png",
        });
        AdditionalItems.Add(new SettingItem
        {
            Title = "About",
            Icon = "about_color.png",
        });
    }
}

