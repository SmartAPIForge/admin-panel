using AdminPanel.Interactors;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace AdminPanel.Navigation;

public partial class MonitoringTab : ContentPage
{
    private bool isSync = false;
    
    public MonitoringTab()
    {
        InitializeComponent();
        ServicePicker.SelectedIndex = 0;
        CreatePlots();
    }

    private async void CreatePlots()
    {
        CreateResponseTimePlot("");
        CreateUnansweredRequestsPlot("");
        SyncXAxis();
    }

    private async void OnServicePickerChanged(object sender, EventArgs e)
    {
        CreatePlots();
    }
    
    private async void CreateResponseTimePlot(string serviceName)
    {
        var model = new PlotModel { 
            Title = "Среднее время ответа",
            TitleColor = OxyColors.WhiteSmoke,
            PlotAreaBorderColor = OxyColors.WhiteSmoke,
        };

        var data = await AnalyticsDataInteractor.GetLatencyRecordsAsync("");
		
        model.Axes.Add(new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "dd.MM HH",
            Minimum = DateTimeAxis.ToDouble(DateTime.Now.AddHours(-23)),
            Maximum = DateTimeAxis.ToDouble(DateTime.Now), 
            IntervalType = DateTimeIntervalType.Hours, 
            TextColor = OxyColors.WhiteSmoke,
            AxislineColor = OxyColors.WhiteSmoke,
            MajorGridlineColor = OxyColors.WhiteSmoke,
            TicklineColor = OxyColors.WhiteSmoke,
            IsZoomEnabled = true,
            IsPanEnabled = true,
        });
		
        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Время (с)",
            Minimum = 0,
            TextColor = OxyColors.WhiteSmoke,
            AxislineColor = OxyColors.WhiteSmoke,
            MajorGridlineColor = OxyColors.WhiteSmoke,
            TicklineColor = OxyColors.WhiteSmoke,
            TitleColor = OxyColors.WhiteSmoke,
            IsZoomEnabled = false,
            IsPanEnabled = false,
        };
        
        model.Axes.Add(yAxis);
        model.Axes[0].AxisChanged += (s, e) =>
        {
            UpdateYAxisMaximum(model, yAxis);
        };

        var lineSeriesList = new List<LineSeries>();
        LineSeries currentSeries = new LineSeries
        {
            Color = OxyColors.Aqua,
            MarkerType = MarkerType.Circle,
        };
        
        TimeSpan expectedInterval = TimeSpan.FromHours(1);
        
        for (int i = 0; i < data.Count; ++i)
        {
            if (i > 0)
            {
                var timeDifference = data[i].Hour - data[i - 1].Hour;
                if (timeDifference > expectedInterval)
                {
                    lineSeriesList.Add(currentSeries);
                    currentSeries = new LineSeries
                    {
                        Color = OxyColors.Aqua,
                        MarkerType = MarkerType.Circle,
                    };
                }
            }

            currentSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(data[i].Hour), data[i].AverageResponseTime));
        }
        lineSeriesList.Add(currentSeries);
        
        foreach (var series in lineSeriesList)
        {
            model.Series.Add(series);
        }

        ResponseTimePlotView.Model = model;
        UpdateYAxisMaximum(ResponseTimePlotView.Model, yAxis);
    }

    private async void CreateUnansweredRequestsPlot(string serviceName)
    {
        var model = new PlotModel { 
            Title = "Среднее количество запросов без ответа",
            TitleColor = OxyColors.WhiteSmoke,
            PlotAreaBorderColor = OxyColors.WhiteSmoke,
        };

        var data = await AnalyticsDataInteractor.GetErrorRecordsAsync(serviceName);
		
        model.Axes.Add(new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "dd.MM HH",
            Minimum = DateTimeAxis.ToDouble(DateTime.Now.AddHours(-23)),
            Maximum = DateTimeAxis.ToDouble(DateTime.Now), 
            IntervalType = DateTimeIntervalType.Hours, 
            TextColor = OxyColors.WhiteSmoke,
            AxislineColor = OxyColors.WhiteSmoke,
            MajorGridlineColor = OxyColors.WhiteSmoke,
            TicklineColor = OxyColors.WhiteSmoke,
            IsZoomEnabled = true,
            IsPanEnabled = true,
        });
		
        var yAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Необработанные запросы (%)",
            Minimum = 0,
            Maximum = 100,
            TextColor = OxyColors.WhiteSmoke,
            AxislineColor = OxyColors.WhiteSmoke,
            MajorGridlineColor = OxyColors.WhiteSmoke,
            TicklineColor = OxyColors.WhiteSmoke,
            TitleColor = OxyColors.WhiteSmoke,
            IsZoomEnabled = false,
            IsPanEnabled = false,
        };
        model.Axes.Add(yAxis);
        model.Axes[0].AxisChanged += (s, e) =>
        {
            UpdateYAxisMaximum(model, yAxis, 20);
        };

        var lineSeriesList = new List<LineSeries>();
        LineSeries currentSeries = new LineSeries
        {
            Color = OxyColors.DarkRed,
            MarkerType = MarkerType.Circle,
        };
        
        TimeSpan expectedInterval = TimeSpan.FromHours(1);
        
        for (int i = 0; i < data.Count; ++i)
        {
            if (i > 0)
            {
                var timeDifference = data[i].Hour - data[i - 1].Hour;
                if (timeDifference > expectedInterval)
                {
                    lineSeriesList.Add(currentSeries);
                    currentSeries = new LineSeries
                    {
                        Color = OxyColors.DarkRed,
                        MarkerType = MarkerType.Circle,
                    };
                }
            }

            currentSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(data[i].Hour), data[i].ErrorPercentage));
        }
        lineSeriesList.Add(currentSeries);
        
        foreach (var series in lineSeriesList)
        {
            model.Series.Add(series);
        }
        
        UnansweredRequestsPlotView.Model = model;
        UpdateYAxisMaximum(UnansweredRequestsPlotView.Model, yAxis, 20);
    }
    
    private void UpdateYAxisMaximum(PlotModel model, LinearAxis yAxis, double defaultValue = 0)
    {
        var timeAxis = model.Axes.OfType<DateTimeAxis>().FirstOrDefault();
        if (timeAxis == null)
            return;
    
        double currentMin = timeAxis.ActualMinimum;
        double currentMax = timeAxis.ActualMaximum;
        DateTime dtMin = DateTimeAxis.ToDateTime(currentMin);
        DateTime dtMax = DateTimeAxis.ToDateTime(currentMax);
    
        double newMax = double.MinValue;
        foreach (var series in model.Series.OfType<LineSeries>())
        {
            foreach (var point in series.Points)
            {
                DateTime pointTime = DateTimeAxis.ToDateTime(point.X);
                if (pointTime >= dtMin && pointTime <= dtMax && point.Y > newMax)
                {
                    newMax = point.Y;
                }
            }
        }
    
        if (newMax > double.MinValue)
            yAxis.Maximum = double.Max(newMax * 1.1, defaultValue);
    
        model.InvalidatePlot(false);
    }
    
    private void SyncXAxis()
    {
        var responseXAxis = ResponseTimePlotView.Model.Axes
            .OfType<DateTimeAxis>()
            .FirstOrDefault();
        var unansweredXAxis = UnansweredRequestsPlotView.Model.Axes
            .OfType<DateTimeAxis>()
            .FirstOrDefault();

        if (responseXAxis == null || unansweredXAxis == null)
            return;
        
        responseXAxis.AxisChanged += (s, e) =>
        {
            if (isSync)
                return;
            isSync= true;
            unansweredXAxis.Zoom(responseXAxis.ActualMinimum, responseXAxis.ActualMaximum);
            UnansweredRequestsPlotView.Model.InvalidatePlot(false);
            isSync = false;
        };
        
        unansweredXAxis.AxisChanged += (s, e) =>
        {
            if (isSync)
                return;
            isSync = true;
            responseXAxis.Zoom(unansweredXAxis.ActualMinimum, unansweredXAxis.ActualMaximum);
            ResponseTimePlotView.Model.InvalidatePlot(false);
            isSync = false;
        };
    }
}