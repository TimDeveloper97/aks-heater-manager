using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
using VstCommon;
using VstCommon.ModelResponses;
using VstService.Models;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Settings
{
    public class StaffListViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<LoginResponse> staffs;
        private double widthCard;

        public ObservableCollection<LoginResponse> Staffs { get => staffs; set => SetProperty(ref staffs, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }
        #endregion

        #region Command  

        public ICommand PageAppearingCommand => new Command(() =>
        {
            Init();
        });

        public ICommand LoadStaffCommand => new Command(async () => await ExecuteLoadStaffCommand());
        #endregion

        public StaffListViewModel()
        {
        }

        #region Method
        void Init()
        {
            Title = Resources.Languages.LanguageResource.settingActionStaff;
            Staffs = new ObservableCollection<LoginResponse>();
            WidthCard = App.ScreenWidth * 0.8;
            IsBusy = true;
        }

        async Task ExecuteLoadStaffCommand()
        {
            IsBusy = true;

            try
            {
                Staffs?.Clear();

                var rdata = new RData
                {
                    Data = new VstRequest { Token = User.Token },
                    Endpoint = API.UserList,
                };

                await _restApiService.VstRequestAPI<List<LoginResponse>>(
                    ERMethod.Post,
                    rdata,
                    onSuccess: (response) =>
                    {
                        foreach (var dr in response.Model)
                        {
                            Staffs.Add(dr);
                        }

                        //Debug.WriteLine(JsonConvert.SerializeObject(response.Model));
                        return Task.CompletedTask;
                    },
                    onFailure: async (e) =>
                    {
                        await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingSnackbarAsync(
                            message: "Loading staff fail.");
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }

        }
        #endregion
    }
}
