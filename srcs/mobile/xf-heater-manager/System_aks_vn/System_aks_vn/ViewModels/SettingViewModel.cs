using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.Settings;
using System_aks_vn.Models.View;
using System_aks_vn.Views;
using System_aks_vn.Views.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace System_aks_vn.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<SettingItem> settingItems, additionalItems;
        private string version, phone;

        public ObservableCollection<SettingItem> SettingItems { get => settingItems; set => SetProperty(ref settingItems, value); }
        public ObservableCollection<SettingItem> AdditionalItems { get => additionalItems; set => SetProperty(ref additionalItems, value); }
        public string Version { get => version; set => SetProperty(ref version, value); }
        public string Phone { get => phone; set => SetProperty(ref phone, value); }
        #endregion

        #region Command  

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });

        public ICommand NavigatorCommand => new Command<SettingItem>(async (item) =>
        {
            if (string.IsNullOrEmpty(item.Page))
                return;

            await Shell.Current.GoToAsync($"{item.Page}");
        });
        #endregion

        public SettingViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = "Settings";
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            GenerateRandomSettings();
            Version = AppInfo.VersionString;
            Phone = "0394852798";
        }

        private void GenerateRandomSettings()
        {
            SettingItems = new ObservableCollection<SettingItem>();
            AdditionalItems = new ObservableCollection<SettingItem>();

            SettingItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingActionProfile,
                Description = Resources.Languages.LanguageResource.settingActionProfileD,
                Page = nameof(AccountPage),
                Icon = "account_color.png",
            });
            SettingItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingActionDevice,
                Description = Resources.Languages.LanguageResource.settingActionDeviceD,
                Page = nameof(DeviceListPage),
                Icon = "device_v2.png",
            });
            SettingItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingActionStaff,
                Description = Resources.Languages.LanguageResource.settingActionStaffD,
                Page = nameof(StaffListPage),
                Icon = "staff_color.png",
            });
            SettingItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingTitle,
                Description = Resources.Languages.LanguageResource.settingActionPasswordD,
                Page = nameof(ProfilePage),
                Icon = "password_color.png",
            });

            AdditionalItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingActionGuide,
                Description = Resources.Languages.LanguageResource.settingActionGuideD,
                Icon = "book.png",
            });
            AdditionalItems.Add(new SettingItem
            {
                Title = Resources.Languages.LanguageResource.settingActionQuestion,
                Description = Resources.Languages.LanguageResource.settingActionQuestionD,
                Icon = "question.png",
            });
        }
        #endregion
    }
}
