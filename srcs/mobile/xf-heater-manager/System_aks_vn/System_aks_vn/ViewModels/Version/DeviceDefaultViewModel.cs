using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using System_aks_vn.ViewModels.Devices;
using System_aks_vn.Views.Devices;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.Version
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    [QueryProperty(nameof(ParameterDeviceName), nameof(ParameterDeviceName))]
    public class DeviceDefaultViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId, parameterDeviceName;
        private DeviceStatusModel deviceStatus;

        public string ParameterDeviceName
        {
            get => parameterDeviceName;
            set
            {
                parameterDeviceName = Uri.UnescapeDataString(value ?? string.Empty);
                Title = parameterDeviceName;
                SetProperty(ref parameterDeviceName, value);
            }
        }
        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                CurrentDeviceId = parameterDeviceId;
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public DeviceStatusModel DeviceStatus { get => deviceStatus; set => SetProperty(ref deviceStatus, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();
        });
        #endregion

        public DeviceDefaultViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            IsBusy = true;
            Title = "";
        }
        #endregion
    }
}
