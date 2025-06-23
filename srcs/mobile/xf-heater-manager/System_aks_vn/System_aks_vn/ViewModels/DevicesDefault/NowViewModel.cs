using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System_aks_vn.Domain;
using System_aks_vn.Models.Response;
using VstCommon;
using VstCommon.ModelResponses;
using VstService.Models;
using Xamarin.Forms;

namespace System_aks_vn.ViewModels.DevicesDefault
{
    public class NowViewModel : BaseViewModel
    {
        #region Property
        private const int MAX_NUMBER = 20; // Maximum number of entries in the chart
        private const double TIME_DELAY = 2.5;

        private float totalPower = 0;
        private Chart chart;
        private CancellationTokenSource _cts;

        public float TotalPower { get => totalPower; set => SetProperty(ref totalPower, value); }
        public Chart Chart { get => chart; set => SetProperty(ref chart, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            //await GetDeviceStatus(DateTime.Now.ToString("HH:mm:ss"));
        });


        #endregion

        public NowViewModel()
        {
            Chart = new LineChart
            {
                Entries = new List<ChartEntry>(),
                //IsAnimated = true,
                AnimationProgress = 1,
                AnimationDuration = new TimeSpan(1200),
                BackgroundColor = SKColor.Parse("#f2f2f2"),
                LabelTextSize = 18,
            };
            _cts = new CancellationTokenSource();
            RunThreadGetPower(_cts.Token);
        }

        #region Method
        void RunThreadGetPower(CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    // TODO: Replace this with your actual logic
                    await GetDeviceStatus(DateTime.Now.ToString("HH:mm:ss"));

                    await Task.Delay((int)TIME_DELAY * 1000, token);
                }
            }, token);
        }

        public void StopThread()
        {
            _cts.Cancel();
        }

        async Task GetDeviceStatus(string time)
        {
            var rdata = new RData
            {
                Data = new VstRequest { Token = User.Token, Value = new { _id = CurrentDeviceId } },
                Endpoint = API.DeviceStatus,
            };

            await _restApiService.VstRequestAPI<DeviceStatusResponse>(
                    ERMethod.Post,
                    rdata,
                    onSuccess: (response) =>
                    {
#if DEBUG
                        var value = response.Model.U * (response.Model.I + (new Random().Next(1, 100)));
#else
                        var value = response.Model.U * (response.Model.I));
#endif
                        TotalPower += value;
                        AddEntryToChart(value, time);

                        //Debug.WriteLine(JsonConvert.SerializeObject(response.Model));
                        return Task.CompletedTask;
                    },
                    onFailure: async (e) =>
                    {
                        await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingSnackbarAsync(
                            message: "Loading device fail.");
                    });
        }

        void AddEntryToChart(float value, string label)
        {
            var entry = new ChartEntry(value)
            {
                Label = label,
                ValueLabel = value.ToString(),
                Color = SKColor.Parse("#3498db")
            };

            if (Chart is LineChart lineChart)
            {
                var updatedEntries = lineChart.Entries.ToList();
                if (updatedEntries.Count() > MAX_NUMBER)
                    updatedEntries.RemoveAt(0); // Remove the oldest entry if we exceed the maximum number

                updatedEntries.Add(entry);
                lineChart.Entries = updatedEntries;
            }
        }
#endregion
    }
}
