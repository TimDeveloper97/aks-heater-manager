using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using maui_heater_manager.Domains;
using maui_heater_manager.Models.Nows;
using Microsoft.Maui.Layouts;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;

namespace maui_heater_manager.ViewModels;

public partial class NowViewModel : BaseViewModel
{
    [ObservableProperty]
    string todayUsage = string.Empty;

    [ObservableProperty]
    bool isCircleVisible = true;

    [ObservableProperty]
    ObservableCollection<HeaterLog> logs = new();

    [ObservableProperty]
    ObservableCollection<CircleModel> circles = new();

    public NowViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "NowViewModel";
        TodayUsage = "1400";

        GenerateRandomLogs();
        GenerateRandomCircles();
        GenerateRandomChart();
    }

    [RelayCommand]
    void ShowCircles() => IsCircleVisible = true;

    [RelayCommand]
    void ShowChart() => IsCircleVisible = false;
}

public partial class NowViewModel
{
    private void GenerateRandomLogs()
    {
        for (int i = 0; i < 3; i++)
        {
            Logs.Add(new HeaterLog
            {
                Title = "Heater Pump Water Heater 2",
                Description = "(Basement) turned on",
                Time = DateTime.Now
            });
            Logs.Add(new HeaterLog
            {
                Title = "Heater Pump Water Heater 2",
                Description = "(Basement) was on for 3 minutes",
                Time = DateTime.Now.AddMinutes(-1),
            });
        }
    }

    private void GenerateRandomCircles()
    {
        Circles.Clear();
        double[] radii = { 50, 100, 80, 120, 60 };
        string[] contents = { "AC8", "Always On", "Unknown", "Heat Pump Water Heater 1", "AC 5" };
        int n = radii.Length;
        double centerX = 150, centerY = 150; // Tâm layout

        // Tìm giá trị lớn nhất của (radii[i] + radii[(i+1)%n])
        double maxSum = 0;
        for (int i = 0; i < n; i++)
        {
            double sum = radii[i] + radii[(i + 1) % n];
            if (sum > maxSum)
                maxSum = sum;
        }

        // Thiết lập giới hạn tìm kiếm cho R:
        double low = maxSum / 2;      // Nếu R thấp hơn mức này thì hai hình tròn không thể tiếp xúc chỉ tại 1 điểm.
        double high = low * 10;       // Giới hạn trên tạm.
        double target = 2 * Math.PI;
        double R = 0;
        const double tolerance = 1e-6;
        for (int iter = 0; iter < 50; iter++)
        {
            double mid = (low + high) / 2;
            double sumAngles = 0;
            for (int i = 0; i < n; i++)
            {
                double ratio = (radii[i] + radii[(i + 1) % n]) / (2 * mid);
                // Clamp ratio để đảm bảo không vượt 1.
                if (ratio > 1) ratio = 1;
                double angle = 2 * Math.Asin(ratio);
                sumAngles += angle;
            }
            if (Math.Abs(sumAngles - target) < tolerance)
            {
                R = mid;
                break;
            }
            if (sumAngles > target)
            {
                // R quá nhỏ
                low = mid;
            }
            else
            {
                high = mid;
            }
            R = mid;
        }

        // Tính các góc giữa các hình tròn dựa trên bán kính tìm được R
        double[] angles = new double[n];
        for (int i = 0; i < n; i++)
        {
            double ratio = (radii[i] + radii[(i + 1) % n]) / (2 * R);
            if (ratio > 1) ratio = 1;
            angles[i] = 2 * Math.Asin(ratio);
        }

        // Đặt các hình tròn lên vòng necklace
        double currentAngle = 0;
        for (int i = 0; i < n; i++)
        {
            double x = centerX + R * Math.Cos(currentAngle);
            double y = centerY + R * Math.Sin(currentAngle);
            Circles.Add(new CircleModel
            {
                Radius = radii[i],
                X = x,
                Y = y,
                Fill = Color.FromArgb($"#{Random.Shared.Next(0x1000000):X6}"),
                Content = contents[i]
            });
            currentAngle += angles[i];
        }
    }
}

public partial class NowViewModel
{
    private readonly Random _r = new();
    private int _delay = 100;
    private ObservableCollection<int> _values;
    private int _current;

    public ISeries[] Series { get; set; }

    public object Sync { get; } = new object();

    public bool IsReading { get; set; } = true;

    private void GenerateRandomChart()
    {
        var items = new List<int>();
        for (var i = 0; i < 1500; i++)
        {
            _current += _r.Next(-9, 10);
            items.Add(_current);
        }

        _values = new ObservableCollection<int>(items);

        // create a series with the data 
        Series = [
            new LineSeries<int>
            {
                Values = _values,
                GeometryFill = null,
                GeometryStroke = null,
                LineSmoothness = 0,
                Stroke = new SolidColorPaint(SKColors.Blue, 1)
            }
        ];

        _delay = 1;
        var readTasks = 10;

        // Finally, we need to start the tasks that will add points to the series. 
        // we are creating {readTasks} tasks 
        // that will add a point every {_delay} milliseconds 
        for (var i = 0; i < readTasks; i++)
        {
            _ = Task.Run(ReadData);
        }
    }

    private async Task ReadData()
    {
        await Task.Delay(1000);

        // to keep this sample simple, we run the next infinite loop 
        // in a real application you should stop the loop/task when the view is disposed 

        while (IsReading)
        {
            await Task.Delay(_delay);

            _current = Interlocked.Add(ref _current, _r.Next(-9, 10));

            lock (Sync)
            {
                _values.Add(_current);
                _values.RemoveAt(0);
            }
        }
    }
}