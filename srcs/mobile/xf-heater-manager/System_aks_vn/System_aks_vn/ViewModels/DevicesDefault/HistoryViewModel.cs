using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;
using System_aks_vn.Domain;
using VstCommon.ModelResponses;
using VstCommon;
using VstService.Models;
using Xamarin.Forms;
using System.Linq;
using System_aks_vn.Models;

namespace System_aks_vn.ViewModels.DevicesDefault
{
    public class HistoryViewModel : BaseViewModel
    {
        #region Property
        private float totalPower = 0;
        private Chart chart;
        private CancellationTokenSource _cts;

        public float TotalPower { get => totalPower; set => SetProperty(ref totalPower, value); }
        public Chart Chart { get => chart; set => SetProperty(ref chart, value); }
        #endregion

        #region Command 
        public ICommand PageAppearingCommand => new Command(async () =>
        {
            await DefaultMode();
        });

        public ICommand ModeCommand => new Command<string>(async (mode) =>
        {
            var number = 0;
            ClearChart();
            var lcoordinates = new List<Coordinate>();

            if (mode == "day")
            {
                number = 24;
                var today = DateTime.Now.Date;
                
                for (int i = 0; i < number; i++)
                {
                    var time = today.AddHours(i);
                    var label = time.ToString("HH:mm dd/MM"); // e.g., 08:00 25/06
                    lcoordinates.Add(new Coordinate
                    {
                        X = label,
                        Y = new Random().Next(1, 100).ToString(),
                    });
                }
            }
            else if (mode == "week")
            {
                number = 7;
                for (int i = 0; i < number; i++)
                {
                    var date = DateTime.Now.Date.AddDays(-number + i + 1);
                    var label = date.ToString("ddd dd/MM"); // e.g., Mon, Tue, ...
                    lcoordinates.Add(new Coordinate
                    {
                        X = label,
                        Y = new Random().Next(1, 100).ToString(),
                    });
                }
            }
            else if (mode == "month")
            {
                number = 30;
                for (int i = 0; i < number; i++)
                {
                    var date = DateTime.Now.Date.AddDays(-number + i + 1);
                    var label = date.ToString("dd/MM"); // e.g., 01/07
                    lcoordinates.Add(new Coordinate
                    {
                        X = label,
                        Y = new Random().Next(1, 100).ToString(),
                    });
                }
            }

            AddEntryToChart(lcoordinates);
        });
        #endregion

        public HistoryViewModel()
        {
            Init();
        }

        #region Method
        void Init()
        {
            Chart = new PointChart
            {
                Entries = new List<ChartEntry>(),
                ValueLabelOrientation = Orientation.Horizontal,
                //IsAnimated = true,
                AnimationProgress = 1,
                AnimationDuration = new TimeSpan(1000),
                BackgroundColor = SKColor.Parse("#f2f2f2"),
                LabelTextSize = 18,
            };
        }

        async Task DefaultMode()
        {
            var lcoordinates = new List<Coordinate>();

            var number = 24;
            var today = DateTime.Now.Date;

            for (int i = 0; i < number; i++)
            {
                var time = today.AddHours(i);
                var label = time.ToString("HH:mm dd/MM"); // e.g., 08:00 25/06
                lcoordinates.Add(new Coordinate
                {
                    X = label,
                    Y = new Random().Next(1, 100).ToString(),
                });
            }

            AddEntryToChart(lcoordinates);
        }

        async Task GetDeviceLogs(string time)
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
                                    var value = response.Model.U * (response.Model.I);
#endif
                        TotalPower += value;

                        //Debug.WriteLine(JsonConvert.SerializeObject(response.Model));
                        return Task.CompletedTask;
                    },
                    onFailure: async (e) =>
                    {
                        await XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.LoadingSnackbarAsync(
                            message: "Loading device fail.");
                    });

        }

        void AddEntryToChart(List<Coordinate> coordinates)
        {
            var lEntries = coordinates.Select(c =>
            {
                TotalPower += float.Parse(c.Y);
                return new ChartEntry(float.Parse(c.Y))
                {
                    Label = c.X,
                    ValueLabel = c.Y,
                    Color = SKColor.Parse("#3498db")
                };
            }).ToList();

            if (Chart is PointChart pointChart)
                pointChart.Entries = lEntries;
        }

        void ClearChart()
        {
            if (Chart is PointChart pointChart)
            {
                pointChart.Entries = new List<ChartEntry>();
            }

            TotalPower = 0;
        }
        #endregion
    }
}
