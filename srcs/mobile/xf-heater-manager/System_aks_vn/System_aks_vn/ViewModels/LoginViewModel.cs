using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
using System_aks_vn.Resources.Languages;
using System_aks_vn.Views;
using VstCommon.ModelResponses;
using VstCommon.Models;
using VstCommon;
using VstService.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using VstService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace System_aks_vn.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Property
        private string serverName, userName, password;
        private string errorServerName, errorUserName, errorPassword, selectedLanguage;
        private ObservableCollection<string> languages;
        private static string _currentLanguage;
        private bool isSaveInfo;

        private IRestApiService _restApiService => App.ServiceProvider.GetService<IRestApiService>();

        public bool IsSaveInfo { get => isSaveInfo; set => SetProperty(ref isSaveInfo, value); }
        public string UserName { get => userName; set => SetProperty(ref userName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string ErrorServerName { get => errorServerName; set => SetProperty(ref errorServerName, value); }
        public string ErrorUserName { get => errorUserName; set => SetProperty(ref errorUserName, value); }
        public string ErrorPassword { get => errorPassword; set => SetProperty(ref errorPassword, value); }
        public ObservableCollection<string> Languages { get => languages; set => SetProperty(ref languages, value); }
        public string SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                SetProperty(ref selectedLanguage, value);

                if (_currentLanguage == SelectedLanguage)
                    return;

                var code = "";
                if (SelectedLanguage == "Vietnamese")
                    code = "vi";
                else if (SelectedLanguage == "Japanese")
                    code = "ja";

                _currentLanguage = selectedLanguage;
                Preferences.Set("language", SelectedLanguage);
                CultureInfo language = new CultureInfo(code);
                Thread.CurrentThread.CurrentUICulture = language;
                LanguageResource.Culture = language;
                Application.Current.MainPage = new AppShell();
            }
        }

        #endregion

        #region Command 
        public ICommand LoginCommand => new Command(async () =>
        {
            if (CheckLogin())
            {
                Reset();
                await ExecuteLoadLoginCommand();
            }
        });
        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });



        #endregion

        public LoginViewModel()
        {

            //ServerName = "aks";
            //UserName = "admin";
            //Password = "Aks@1234";
        }

        #region Method
        void Init()
        {
            Title = "Login";
            Languages = new ObservableCollection<string> { "English", "Vietnamese", "Japanese" };
            SelectedLanguage = Preferences.Get("language", "English");
            IsSaveInfo = Preferences.Get("saveinfo", false);

            if(IsSaveInfo)
            {
                UserName = Preferences.Get("username", null);
                Password = Preferences.Get("password", null);
            }    

            //if (Mqtt.IsConnected == false)
            //    Mqtt.Connect();

            Reset();
        }

        void Reset()
        {
            ErrorServerName = null;
            ErrorUserName = null;
            ErrorPassword = null;
        }

        bool CheckLogin()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                ErrorUserName = "User name is required.";
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                ErrorPassword = "Password is required.";
                return false;
            }

            return true;
        }

        async Task ExecuteLoadLoginCommand()
        {
            var loadingDialog = await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance
                    .LoadingDialogAsync(message: $"Waiting to login...");
            IsBusy = true;

            if (IsSaveInfo)
            {
                Preferences.Set("username", UserName);
                Preferences.Set("password", Password);
            }
            Preferences.Set("saveinfo", IsSaveInfo);

            try
            {
                var x = _restApiService;

                var rdata = new RData
                {
                    Data = new VstRequest { Value = new LoginRequest { Password = Password, Username = UserName } },
                    Endpoint = API.Login,
                };

                await _restApiService.VstRequestAPI<LoginResponse>(
                    ERMethod.Post,
                    rdata,
                    onSuccess: async (response) =>
                    {
                        API.UserType = response.Model?.Role?.ToLower() ?? API.CUSTOMER;
                        User = response.Model;

                        await Shell.Current.GoToAsync(nameof(HomePage));
                    },
                    onFailure: async (e) =>
                    {
                        await Shell.Current.DisplayAlert("Information", "Login fail", "Cancel");
                    });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                await loadingDialog.DismissAsync();
                IsBusy = false;
            }
        }
        #endregion
    }
}
