using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_aks_vn.Views;
using System_aks_vn.Views.Devices;
using System_aks_vn.Views.Devices.Settings;
using System_aks_vn.Views.Settings;
using System_aks_vn.Views.Version;
using VstService.Interfaces;
using VstService.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace System_aks_vn
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {

        /// <summary>
        /// https://devblogs.microsoft.com/xamarin/microcharts-elegant-cross-platform-charts-for-any-app/
        /// https://github.com/microcharts-dotnet/Microcharts/wiki/LineChart
        /// https://github.com/microcharts-dotnet/Microcharts
        /// 
        /// 
        /// https://github.com/thudugala/Plugin.LocalNotification/wiki/2.-Usage-10.0.0-Xamarin.Forms-(Support-ended-on-May-1,-2024)
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            InitRoute();
        }

        void InitRoute()
        {
            Routing.RegisterRoute(nameof(DeviceV30Page), typeof(DeviceV30Page));
            Routing.RegisterRoute(nameof(DeviceDefaultPage), typeof(DeviceDefaultPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(DeviceSettingPage), typeof(DeviceSettingPage));
            Routing.RegisterRoute(nameof(DeviceHistoryPage), typeof(DeviceHistoryPage));
            Routing.RegisterRoute(nameof(DeviceSettingSmsPage), typeof(DeviceSettingSmsPage));
            Routing.RegisterRoute(nameof(DeviceSettingCallPage), typeof(DeviceSettingCallPage));
            Routing.RegisterRoute(nameof(DeviceSettingSchedulePage), typeof(DeviceSettingSchedulePage));

            Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
            Routing.RegisterRoute(nameof(DeviceListPage), typeof(DeviceListPage));
            Routing.RegisterRoute(nameof(StaffListPage), typeof(StaffListPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
