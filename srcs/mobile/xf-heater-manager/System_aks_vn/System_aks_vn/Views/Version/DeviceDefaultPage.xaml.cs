using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace System_aks_vn.Views.Version
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceDefaultPage : Xamarin.Forms.TabbedPage
    {
        public DeviceDefaultPage()
        {
            InitializeComponent();

            // Set the toolbar to the bottom on Android
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            // Optional: set tab icons to be always shown
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(true);
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSmoothScrollEnabled(true);
        }
    }
}