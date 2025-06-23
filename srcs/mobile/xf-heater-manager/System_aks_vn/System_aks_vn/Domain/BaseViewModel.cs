
using Microsoft.Extensions.DependencyInjection;
using Plugin.LocalNotification;
using System;
using System.Threading.Tasks;
using VstCommon.ModelResponses;
using VstService.Interfaces;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace System_aks_vn.Domain
{
    public class BaseViewModel : BaseBinding
    {
        protected IRestApiService _restApiService => App.ServiceProvider.GetService<IRestApiService>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #region Extend
        protected static LoginResponse User { get; set; }

        protected static string CurrentDeviceId { get; set; }

        protected async Task TimeoutSession(string message)
        {

            await MaterialDialog.Instance.SnackbarAsync(message: message,
                              msDuration: MaterialSnackbar.DurationLong);
            await Shell.Current.GoToAsync("//LoginPage");
        }
        #endregion

        #region Notification
        protected async Task ShowNotification(string title, string description, string returnData = null)
        {
            var random = new Random();
            var id = random.Next(1, int.MaxValue);

            var notification = new NotificationRequest
            {
                NotificationId = id,
                Title = title,
                Description = description,
                ReturningData = returnData, // Returning data when tapped on notification.
            };

            await NotificationCenter.Current.Show(notification);
        }
        #endregion
    }
}
