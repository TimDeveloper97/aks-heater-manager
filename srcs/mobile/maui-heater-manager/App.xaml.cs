namespace maui_heater_manager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Application.Current.UserAppTheme = AppTheme.Light; // Luôn Dark mode
            MainPage.WidthRequest = 400; // Set a fixed width for the main page
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

#if WINDOWS
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                window.Width = 500;
            }
#endif

            return window;
        }
    }
}
