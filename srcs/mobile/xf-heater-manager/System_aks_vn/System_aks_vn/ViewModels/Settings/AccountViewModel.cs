using System.Windows.Input;
using System_aks_vn.Domain;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Settings
{
    public class AccountViewModel : BaseViewModel
    {
        #region Property

        #endregion

        #region Command  

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        #endregion

        public AccountViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = Resources.Languages.LanguageResource.settingActionProfile;
        }

        #endregion
    }
}
