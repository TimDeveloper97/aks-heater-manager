using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class DeviceListViewModel : BaseViewModel
    {
        #region Property
        private ObservableCollection<DeviceModel> devices;
        private double widthCard;

        public ObservableCollection<DeviceModel> Devices { get => devices; set => SetProperty(ref devices, value); }
        public double WidthCard { get => widthCard; set => SetProperty(ref widthCard, value); }
        #endregion

        #region Command  

        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();
        });

        public ICommand LoadDeviceCommand { get; set; }
        #endregion

        public DeviceListViewModel()
        {
            LoadDeviceCommand = new Command(async () => await ExecuteLoadDeviceCommand());
        }

        #region Method
        void Init()
        {
            Title = Resources.Languages.LanguageResource.settingActionDevice;
            Devices = new ObservableCollection<DeviceModel>();
            WidthCard = App.ScreenWidth * 0.8;
        }

        async Task ExecuteLoadDeviceCommand()
        {
            IsBusy = true;

            try
            {
                Devices?.Clear();

                var rdata = new RData
                {
                    Data = new VstRequest { Token = User.Token },
                    Endpoint = API.DeviceList,
                };

                await _restApiService.VstRequestAPI<List<DeviceResponse>>(
                    ERMethod.Post,
                    rdata,
                    onSuccess: (response) =>
                    {
                        foreach (var dr in response.Model)
                        {
                            Devices.Add(new DeviceModel(dr));
                        }

                        //Debug.WriteLine(JsonConvert.SerializeObject(response.Model));
                        return Task.CompletedTask;
                    },
                    onFailure: async (e) =>
                    {
                        await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingSnackbarAsync(
                            message: "Loading device fail.");
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
