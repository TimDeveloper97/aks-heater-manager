using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System_aks_vn.Domain;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Settings
{
    public class StaffListViewModel : BaseViewModel
    {
        #region Property

        #endregion

        #region Command  

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });
        #endregion

        public StaffListViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = Resources.Languages.LanguageResource.settingActionStaff;
        }


        #endregion
    }
}
