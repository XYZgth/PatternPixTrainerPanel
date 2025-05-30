using Common;
using Prism.Events;
using PatternPixTrainerPanel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PatternPixTrainerPanel.Data;
using System.Windows;
using Common.Command;
using System.Windows.Input;
using PatternPixTrainerPanel.Events;

namespace PatternPixTrainerPanel.ViewModel
{
    public class AnalysisViewModel : BaseViewModel
    {

        public AnalysisViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {


            FromDate = DateTime.Today.AddDays(-30);
            ToDate = DateTime.Today;
            FromTime = TimeSpan.Zero;
            ToTime = new TimeSpan(23, 59, 59);

            Children = new ObservableCollection<string>();
            SymmetryOptions = new ObservableCollection<string> { "All", "H", "V", "B", "R", "?" };
            ChartTypes = new ObservableCollection<string> { "Errors/Time", "Errors Only", "Time Only" };

            ChartData = new ObservableCollection<SessionDataPoint>();

            SymmetryStatsData = new ObservableCollection<SymmetryStats>();

            SelectedChild = string.Empty;
            SelectedSymmetry = "All";
            SelectedChartType = "Errors/Time";

            // Asynchron initialisieren
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadChildrenAsync();
            await ApplyFilterAsync();
        }

        private async Task LoadChildrenAsync()
        {
            try
            {
                using var context = new PatternPixDbContext();
                var children = await context.Children
                    .OrderBy(c => c.FirstName)
                    .ThenBy(c => c.LastName)
                    .Select(c => c.FullName)
                    .ToListAsync();

                Children.Clear();
                foreach (var child in children)
                {
                    Children.Add(child);
                }
            }
            catch (Exception ex)
            {
                // Logging oder Error Handling hier
                Console.WriteLine($"Fehler beim Laden der Kinder: {ex.Message}");
            }
        }

        private string _selectedChild;
        public string SelectedChild
        {
            get => _selectedChild;
            set
            {
                if (_selectedChild != value)
                {
                    _selectedChild = value;
                    OnPropertyChanged(nameof(SelectedChild));
                    _ = ApplyFilterAsync();
                }
            }
        }

        private string _selectedSymmetry;
        public string SelectedSymmetry
        {
            get => _selectedSymmetry;
            set
            {
                if (_selectedSymmetry != value)
                {
                    _selectedSymmetry = value;
                    OnPropertyChanged(nameof(SelectedSymmetry));
                    _ = ApplyFilterAsync();
                }
            }
        }

        private string _selectedChartType;
        public string SelectedChartType
        {
            get => _selectedChartType;
            set
            {
                if (_selectedChartType != value)
                {
                    _selectedChartType = value;
                    OnPropertyChanged(nameof(SelectedChartType));
                    OnPropertyChanged(nameof(SelectedChartType));

                    // Chart-spezifische Properties benachrichtigen
                    OnPropertyChanged(nameof(PrimaryChartTitle));
                    OnPropertyChanged(nameof(PrimaryYAxisHeader));
                    OnPropertyChanged(nameof(PrimaryDataPath));
                    OnPropertyChanged(nameof(SecondaryDataPath));
                    OnPropertyChanged(nameof(PrimarySeriesLabel));
                    OnPropertyChanged(nameof(SecondarySeriesLabel));
                    OnPropertyChanged(nameof(ShowSecondarySeriesVisibility));
                }
            }
        }

        private DateTime _fromDate;
        public DateTime FromDate
        {
            get => _fromDate;
            set
            {
                if (_fromDate != value)
                {
                    _fromDate = value;
                    OnPropertyChanged(nameof(FromDate));
                    _ = ApplyFilterAsync();
                }
            }
        }

        private DateTime _toDate;
        public DateTime ToDate
        {
            get => _toDate;
            set
            {
                if (_toDate != value)
                {
                    _toDate = value;
                    OnPropertyChanged(nameof(ToDate));
                    _ = ApplyFilterAsync();
                }
            }
        }

        private TimeSpan _fromTime;
        public TimeSpan FromTime
        {
            get => _fromTime;
            set
            {
                if (_fromTime != value)
                {
                    _fromTime = value;
                    OnPropertyChanged(nameof(FromTime));
                    _ = ApplyFilterAsync();
                }
            }
        }

        private TimeSpan _toTime;
        public TimeSpan ToTime
        {
            get => _toTime;
            set
            {
                if (_toTime != value)
                {
                    _toTime = value;
                    OnPropertyChanged(nameof(ToTime));
                    _ = ApplyFilterAsync();
                }
            }
        }

        public ObservableCollection<string> Children { get; }
        public ObservableCollection<string> SymmetryOptions { get; }
        public ObservableCollection<string> ChartTypes { get; }

        public ObservableCollection<SessionDataPoint> ChartData { get; }

        public ObservableCollection<SymmetryStats> SymmetryStatsData { get; }

        private int _totalSessions;
        public int TotalSessions
        {
            get => _totalSessions;
            set
            {
                if (_totalSessions != value)
                {
                    _totalSessions = value;
                    OnPropertyChanged(nameof(TotalSessions));
                }
            }
        }

        private double _averageTime;
        public double AverageTime
        {
            get => _averageTime;
            set
            {
                if (_averageTime != value)
                {
                    _averageTime = value;
                    OnPropertyChanged(nameof(AverageTime));
                }
            }
        }

        private double _averageErrors;
        public double AverageErrors
        {
            get => _averageErrors;
            set
            {
                if (_averageErrors != value)
                {
                    _averageErrors = value;
                    OnPropertyChanged(nameof(AverageErrors));
                }
            }
        }

        private double _bestTime;
        public double BestTime
        {
            get => _bestTime;
            set
            {
                if (_bestTime != value)
                {
                    _bestTime = value;
                    OnPropertyChanged(nameof(BestTime));
                }
            }
        }





        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        private async Task ApplyFilterAsync()
        {
            IsLoading = true;

            try
            {
                using var context = new PatternPixDbContext();

                var query = context.Trainings
                    .Include(t => t.Child)
                    .AsQueryable();

                // Filter nach Kind anwenden
                if (!string.IsNullOrEmpty(SelectedChild))
                {
                    query = query.Where(t => (t.Child.FirstName + " " + t.Child.LastName) == SelectedChild);
                }

                // Filter nach Symmetrie anwenden
                if (SelectedSymmetry != "All")
                {
                    query = query.Where(t => t.Symmetry == SelectedSymmetry);
                }

                // Datumsfilter anwenden
                query = query.Where(t =>
                    t.Date.Date >= FromDate.Date &&
                    t.Date.Date <= ToDate.Date &&
                    t.TimeOfDay >= FromTime &&
                    t.TimeOfDay <= ToTime);

                // Sortierung nach Datum
                query = query.OrderBy(t => t.Date).ThenBy(t => t.TimeOfDay);

                var filteredData = await query.ToListAsync();

                await UpdateStatisticsAsync(filteredData);
                await UpdateChartDataAsync(filteredData);
                await UpdateSymmetryStatsAsync(filteredData);
                await UpdateMovingAverageAsync(filteredData);

            }
            catch (Exception ex)
            {
                // Logging oder Error Handling hier
                Console.WriteLine($"Fehler beim Anwenden der Filter: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateStatisticsAsync(List<Training> data)
        {
            await Task.Run(() =>
            {
                TotalSessions = data.Count;
                AverageTime = data.Any() ? data.Average(x => x.TimeNeeded) : 0;
                AverageErrors = data.Any() ? data.Average(x => x.Errors) : 0;
                BestTime = data.Any() ? data.Min(x => x.TimeNeeded) : 0;

            });
        }




        // Für manuelle Aktualisierung
        public async Task RefreshDataAsync()
        {
            await LoadChildrenAsync();
            await ApplyFilterAsync();
        }


        public DateTime StartDate
        {
            get => FromDate;
            set => FromDate = value;
        }

        public DateTime EndDate
        {
            get => ToDate;
            set => ToDate = value;
        }

        // Time range options and selection
        private ObservableCollection<string> _timeRanges;
        public ObservableCollection<string> TimeRanges
        {
            get => _timeRanges ??= new ObservableCollection<string>
    {
        "Entire Day",
        "Morning (6:00-12:00)",
        "Afternoon (12:00-18:00)",
        "Evening (18:00-24:00)"
    };
        }

        private string _selectedTimeRange = "Entire Day";
        public string SelectedTimeRange
        {
            get => _selectedTimeRange;
            set
            {
                if (_selectedTimeRange != value)
                {
                    _selectedTimeRange = value;
                    OnPropertyChanged(nameof(SelectedTimeRange));
                    UpdateTimeRangeFilter(value);
                    _ = ApplyFilterAsync();
                }
            }
        }

        public ObservableCollection<Training> Trainings => new ObservableCollection<Training>(ChartData.Select(cd => new Training
        {
            Date = DateTime.Parse(cd.Label + DateTime.Now.Year),
            Errors = cd.Errors,
            TimeNeeded = cd.TimeNeeded,

        }).ToList());

        public ObservableCollection<Training> FilteredTrainings => Trainings;

        // Chart configuration properties
        public string PrimaryChartTitle
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors Only" => "Error Analysis",
                    "Time Only" => "Time Analysis",
                    _ => "Training Performance Analysis"
                };
            }
        }

        public string PrimaryYAxisHeader
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors Only" => "Errors",
                    "Time Only" => "Time (seconds)",
                    _ => "Performance Metrics"
                };
            }
        }

        public string PrimaryDataPath
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors Only" => "Errors",
                    "Time Only" => "TimeNeeded",
                    _ => "TimeNeeded"
                };
            }
        }

        public string SecondaryDataPath
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors/Time" => "Errors",
                    _ => null
                };
            }
        }

        public string PrimarySeriesLabel
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors Only" => "Errors",
                    "Time Only" => "Time",
                    _ => "Time"
                };
            }
        }

        public string SecondarySeriesLabel
        {
            get
            {
                return SelectedChartType switch
                {
                    "Errors/Time" => "Errors",
                    _ => ""
                };
            }
        }

        public Visibility ShowSecondarySeriesVisibility
        {
            get => SelectedChartType == "Errors/Time" ? Visibility.Visible : Visibility.Collapsed;
        }

        // Statistics properties that match XAML bindings
        public ObservableCollection<SymmetryStatistic> SymmetryStats =>
            new ObservableCollection<SymmetryStatistic>(SymmetryStatsData.Select(s => new SymmetryStatistic
            {
                SymmetryType = s.Symmetry,
                AverageTime = s.AvgTime,
                AverageErrors = s.AvgErrors
            }));

        // Moving average data for trend chart
        public ObservableCollection<MovingAveragePoint> MovingAverageData { get; } = new();



        private void UpdateTimeRangeFilter(string timeRange)
        {
            switch (timeRange)
            {
                case "Morning (6:00-12:00)":
                    FromTime = new TimeSpan(6, 0, 0);
                    ToTime = new TimeSpan(12, 0, 0);
                    break;
                case "Afternoon (12:00-18:00)":
                    FromTime = new TimeSpan(12, 0, 0);
                    ToTime = new TimeSpan(18, 0, 0);
                    break;
                case "Evening (18:00-24:00)":
                    FromTime = new TimeSpan(18, 0, 0);
                    ToTime = new TimeSpan(23, 59, 59);
                    break;
                default: // "Entire Day"
                    FromTime = TimeSpan.Zero;
                    ToTime = new TimeSpan(23, 59, 59);
                    break;
            }
        }

        // Update existing methods to notify property changes for chart bindings
        private async Task UpdateChartDataAsync(List<Training> data)
        {
            await Task.Run(() =>
            {
                var chartPoints = data.Select(d => new SessionDataPoint
                {
                    Label = d.Date.ToString("dd.MM."),
                    Errors = d.Errors,
                    TimeNeeded = d.TimeNeeded
                }).ToList();

                // UI Update im UI-Thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    ChartData.Clear();
                    foreach (var point in chartPoints)
                    {
                        ChartData.Add(point);
                    }


                    // Notify the binding property
                    OnPropertyChanged(nameof(SymmetryStats));

                    // Notify chart-related properties
                    OnPropertyChanged(nameof(Trainings));
                    OnPropertyChanged(nameof(FilteredTrainings));
                    OnPropertyChanged(nameof(PrimaryChartTitle));
                    OnPropertyChanged(nameof(PrimaryYAxisHeader));
                    OnPropertyChanged(nameof(PrimaryDataPath));
                    OnPropertyChanged(nameof(SecondaryDataPath));
                    OnPropertyChanged(nameof(PrimarySeriesLabel));
                    OnPropertyChanged(nameof(SecondarySeriesLabel));
                    OnPropertyChanged(nameof(ShowSecondarySeriesVisibility));

                });
            });
        }


        // Update the moving average method to use the new data structure
        private async Task UpdateMovingAverageAsync(List<Training> data)
        {
            await Task.Run(() =>
            {
                var trendData = new List<MovingAveragePoint>();
                const int windowSize = 5;

                for (int i = 0; i <= data.Count - windowSize; i++)
                {
                    var window = data.Skip(i).Take(windowSize);
                    trendData.Add(new MovingAveragePoint
                    {
                        SessionNumber = i + 1,
                        AverageErrors = window.Average(x => x.Errors),
                        AverageTime = window.Average(x => x.TimeNeeded)
                    });
                }

                // UI Update im UI-Thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    MovingAverageData.Clear();
                    foreach (var trend in trendData)
                    {
                        MovingAverageData.Add(trend);
                    }
                });
            });
        }

        // Update SymmetryStats method to notify the binding
        private async Task UpdateSymmetryStatsAsync(List<Training> data)
        {
            await Task.Run(() =>
            {
                var grouped = data.GroupBy(x => x.Symmetry);
                var statsData = grouped.Select(g => new SymmetryStats
                {
                    Symmetry = g.Key.ToString(),
                    AvgTime = g.Average(x => x.TimeNeeded),
                    AvgErrors = g.Average(x => x.Errors)
                }).ToList();

                // UI Update im UI-Thread
                App.Current.Dispatcher.Invoke(() =>
                {
                    SymmetryStatsData.Clear();
                    foreach (var stat in statsData)
                    {
                        SymmetryStatsData.Add(stat);
                    }

                    // Notify the binding property
                    OnPropertyChanged(nameof(SymmetryStats));
                });
            });
        }
        /// \brief Befehl zum Zurücknavigieren zur Hauptansicht.
        private ICommand _backCommand;

        /**
         * \brief Property für den BackCommand.
         *
         * Dieser Befehl navigiert zur "MainView", wenn er ausgeführt wird.
         * 
         * \return Ein ICommand, das bei Ausführung ein Navigationsevent publiziert.
         */
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new ActionCommand(
                        param => EventAggregator.GetEvent<NavigationEvent>().Publish("MainView"),
                        param => true);
                }
                return _backCommand;
            }
        }
    }
}




public class SymmetryStatistic
{
    public string SymmetryType { get; set; }
    public double AverageTime { get; set; }
    public double AverageErrors { get; set; }
}

public class MovingAveragePoint
{
    public int SessionNumber { get; set; }
    public double AverageTime { get; set; }
    public double AverageErrors { get; set; }
}

