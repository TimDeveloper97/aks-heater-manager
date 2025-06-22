﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Controls;
using System_aks_vn.Domain;
using System_aks_vn.Models.View;
using System_aks_vn.ViewModels.Devices;
using System_aks_vn.Views.Devices;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace System_aks_vn.ViewModels.Version
{
    [QueryProperty(nameof(ParameterDeviceId), nameof(ParameterDeviceId))]
    public class DeviceV30ViewModel : BaseViewModel
    {
        #region Property
        private string parameterDeviceId;
        private DeviceStatusModel deviceStatus;

        public string ParameterDeviceId
        {
            get => parameterDeviceId;
            set
            {
                parameterDeviceId = Uri.UnescapeDataString(value ?? string.Empty);
                Title = parameterDeviceId;
                SetProperty(ref parameterDeviceId, value);
            }
        }
        public DeviceStatusModel DeviceStatus { get => deviceStatus; set => SetProperty(ref deviceStatus, value); }

        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            Init();

            await ExecuteLoadDeviceStatusCommand();
        });

        public ICommand StatusCommand => new Command<string>((tag) =>
        {
            //Mqtt.ClearEvent();
            //Mqtt.Publish(Topic, new DeviceContext
            //{
            //    DeviceId = ParameterDeviceId,
            //    Url = Api.Control,
            //    Token = Token,
            //    Value = tag,
            //});

            //Mqtt.MessageReceived += async (s, e) =>
            //{
            //    var res = (s as Mqtt).Response;
            //    if (res.Code != 0)
            //        await MaterialDialog.Instance.SnackbarAsync(message: res.Message,
            //                  msDuration: MaterialSnackbar.DurationLong);
            //};
        });

        public ICommand DeviceSettingCommand => new Command<string>(async (deviceId) =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceSettingPage)}" +
                $"?{nameof(DeviceSettingViewModel.ParameterDeviceId)}={deviceId}");
        });

        public ICommand DeviceHistoryCommand => new Command<string>(async (deviceId) =>
        {
            await Shell.Current.GoToAsync($"{nameof(DeviceHistoryPage)}" +
                $"?{nameof(DeviceHistoryViewModel.ParameterDeviceId)}={deviceId}");
        });
        #endregion

        public DeviceV30ViewModel()
        {
        }

        #region Method
        void Init()
        {
            DependencyService.Get<IStatusBar>().SetColoredStatusBar("#007bff");
            DeviceStatus = new DeviceStatusModel();
            IsBusy = true;
        }

        Task ExecuteLoadDeviceStatusCommand()
        {
            IsBusy = true;

            try
            {
                DependencyService.Get<ITimer>().StartTimer(TimeSpan.FromSeconds(3), () =>
                {
                    //if (!Mqtt.IsConnected)
                    //    Mqtt.Connect();

                    //Mqtt.ClearEvent();
                    //Mqtt.Publish(Topic, new DeviceContext
                    //{
                    //    DeviceId = ParameterDeviceId,
                    //    Url = Api.DeviceStatus,
                    //});

                    //Mqtt.MessageReceived += async (s, e) =>
                    //{
                    //    try
                    //    {
                    //        var res = (s as Mqtt).Response;
                    //        if (res.Code == 100)
                    //            await TimeoutSession(res.Message);

                    //        if (res.Count == 2 || res.Value == null)
                    //            return;

                    //        var status = JObject.Parse(res.Value.ToString());
                    //        var tmp = new DeviceStatusModel();

                    //        foreach (var tag in DeviceStatusModel.Tags)
                    //        {
                    //            var value = status[tag]?.ToString();
                    //            if (!string.IsNullOrEmpty(value))
                    //            {
                    //                Type myType = tmp.GetType();
                    //                var props = new List<PropertyInfo>(myType.GetProperties());

                    //                foreach (PropertyInfo prop in props)
                    //                {
                    //                    var propTag = prop.GetCustomAttribute<DescriptionAttribute>()?.Description;
                    //                    var propValue = bool.Parse(prop.GetValue(tmp).ToString());
                    //                    if (propTag != null && propTag == tag)
                    //                    {
                    //                        // neu 2 trang thai khac nhau
                    //                        if (propValue == false)
                    //                            prop.SetValue(tmp, true);
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        DeviceStatus = tmp;
                    //    }
                    //    catch (Exception) { }
                    //};

                    return true; // True = Repeat again, False = Stop the timer
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

            return Task.CompletedTask;
        }
        #endregion
    }
}
