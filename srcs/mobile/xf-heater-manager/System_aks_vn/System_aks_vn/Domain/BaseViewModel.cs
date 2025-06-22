
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VstCommon.ModelResponses;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace System_aks_vn.Domain
{
    public class BaseViewModel : BaseBinding
    {
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
        protected LoginResponse User { get; set; }

        protected async Task TimeoutSession(string message)
        {

            await MaterialDialog.Instance.SnackbarAsync(message: message,
                              msDuration: MaterialSnackbar.DurationLong);
            await Shell.Current.GoToAsync("//LoginPage");
        }
        #endregion
    }
}
