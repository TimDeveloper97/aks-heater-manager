using Microsoft.Extensions.DependencyInjection;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System;
using VstService.Interfaces;
using VstService.Repositories;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace System_aks_vn
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            InitializeComponent();

            InitDependencyService();

            XF.Material.Forms.Material.Init(this);
            MainPage = new AppShell();
        }

        void InitDependencyService()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IWebApiRepository, WebApiRepository>();
            services.AddSingleton<IRestApiService, RestApiService>();
            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
