using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_heater_manager.Domains;
using maui_heater_manager.Pages.Mains;
using Microsoft.Extensions.DependencyInjection;

namespace maui_heater_manager.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    ContentView content;

    [ObservableProperty]
    int selectedTab = 1;

    public MainViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "MainViewModel";

        Content = new NowsView(serviceProvider);
        this._serviceProvider = serviceProvider;
    }

    [RelayCommand]
    async Task Back()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task Navigate(string page)
    {
        if(page == "Now")
        {
            Content = new NowsView(_serviceProvider);
            SelectedTab = 1;
        }    
        else if (page == "Device")
        {
            Content = new DevicesView(_serviceProvider);
            SelectedTab = 3;
        }    
        else if (page == "Usage")
        {
            Content = new HistoryView(_serviceProvider);
            SelectedTab = 2;
        }    
        else if (page == "Setting")
        {
            Content = new SettingView(_serviceProvider);
            SelectedTab = 4;
        }    
    }
}
