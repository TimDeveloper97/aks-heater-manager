using CommunityToolkit.Mvvm.ComponentModel;
using maui_heater_manager.Domains;
using maui_heater_manager.Models.Nows;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.ObjectModel;

namespace maui_heater_manager.ViewModels;

public partial class NowViewModel : BaseViewModel
{
    [ObservableProperty]
    string todayUsage = string.Empty;

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
    }
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