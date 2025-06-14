using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using maui_heater_manager.Domains;
using SkiaSharp;

namespace maui_heater_manager.ViewModels;

public partial class UsageViewModel : BaseViewModel
{
    public ISeries[] SeriesTrend { get; set; } = [
        new ColumnSeries<double>
        {
            Values = [ 20, 50, 40, 20, 40, 30, 50 ],

            // Defines the distance between every bars in the series
            Padding = 10,

            // Defines the max width a bar can have
            MaxBarWidth = double.MaxValue,
            
        }
    ];


    public Axis[] YAxes { get; set; } = [
        new Axis
        {
            SeparatorsPaint = new SolidColorPaint(SKColors.Gray, 1)
        }
    ];

    // Các ngày trong tuần
    public Axis[] XAxesTrend { get; set; } = [
        new Axis
        {
            Labels = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
            LabelsPaint = new SolidColorPaint(SKColors.Black)
        }
    ];

    public double TotalTrend => ((ColumnSeries<double>)SeriesTrend[0]).Values?.Sum() ?? 0;

    public UsageViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Title = "UsageViewModel";

        GenerateRandomPower();
    }
}

public partial class UsageViewModel
{
    private readonly Random _random = new();

    public ISeries[] SeriesPower { get; set; }

    public ICartesianAxis[] XAxesPower { get; set; }

    public double TotalPower { get; set; }

    private void GenerateRandomPower()
    {
        var trend = 100;
        var values = new List<int>();

        for (var i = 0; i < 100; i++)
        {
            trend += _random.Next(-30, 50);
            values.Add(trend);
        }

        TotalPower = values.Sum();
        SeriesPower = [new ColumnSeries<int>(values)];
        XAxesPower = [new Axis()];
        XAxesPower[0].MinLimit = null;
        XAxesPower[0].MaxLimit = null;
    }
}
