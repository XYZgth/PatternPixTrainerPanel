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
    /**
     * \brief ViewModel für die Analyse-Ansicht der PatternPix Trainer Anwendung.
     * 
     * Diese Klasse verwaltet die Datenanalyse und Statistiken für Trainingseinheiten,
     * einschließlich Filterung, Peer-Vergleiche und Chart-Darstellung.
     */
    public class AnalysisViewModel : BaseViewModel
    {
        /**
         * \brief Konstruktor der AnalysisViewModel-Klasse.
         * 
         * Initialisiert alle Collections, setzt Standardwerte für Filterparameter
         * und startet die asynchrone Initialisierung.
         * 
         * \param eventAggregator Der Event-Aggregator für die Kommunikation zwischen ViewModels.
         */
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

            Trainings = new ObservableCollection<Training>();
            SymmetryStatsData = new ObservableCollection<SymmetryStats>();

            PeerComparisonData = new ObservableCollection<PeerComparisonData>();
            PeerSymmetryComparison = new ObservableCollection<PeerSymmetryComparison>();

            SelectedChild = string.Empty;
            SelectedSymmetry = "All";
            SelectedChartType = "Errors/Time";

            // Asynchron initialisieren
            _ = InitializeAsync();
        }

        /**
         * \brief Asynchrone Initialisierung des ViewModels.
         * 
         * Lädt die Kinderliste und wendet die Standardfilter an.
         */
        private async Task InitializeAsync()
        {
            await LoadChildrenAsync();
            await ApplyFilterAsync();
        }

        /**
         * \brief Lädt die Liste aller Kinder aus der Datenbank.
         * 
         * Holt alle Kinder aus der Datenbank, sortiert sie alphabetisch
         * und fügt sie zur Children-Collection hinzu.
         */
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
                Console.WriteLine($"Fehler beim Laden der Kinder: {ex.Message}");
            }
        }

        private string _selectedChild;
        /**
         * \brief Property für das aktuell ausgewählte Kind.
         * 
         * Wenn ein neues Kind ausgewählt wird, werden automatisch die Filter
         * angewendet und die Daten aktualisiert.
         * 
         * \return Der Name des ausgewählten Kindes als String.
         */
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
        /**
         * \brief Property für die aktuell ausgewählte Symmetrie-Option.
         * 
         * Filtert die Trainingsdaten nach der gewählten Symmetrie-Art.
         * 
         * \return Die ausgewählte Symmetrie-Option als String.
         */
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
        /**
         * \brief Property für den aktuell ausgewählten Chart-Typ.
         * 
         * Bestimmt welche Art von Chart angezeigt wird (Fehler/Zeit, nur Fehler, nur Zeit).
         * Aktualisiert automatisch alle chart-spezifischen Properties.
         * 
         * \return Der ausgewählte Chart-Typ als String.
         */
        public string SelectedChartType
        {
            get => _selectedChartType;
            set
            {
                if (_selectedChartType != value)
                {
                    _selectedChartType = value;
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
        /**
         * \brief Property für das Startdatum des Zeitfilters.
         * 
         * Bestimmt den Beginn des Zeitraums für die Datenfilterung.
         * 
         * \return Das Startdatum als DateTime.
         */
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
        /**
         * \brief Property für das Enddatum des Zeitfilters.
         * 
         * Bestimmt das Ende des Zeitraums für die Datenfilterung.
         * 
         * \return Das Enddatum als DateTime.
         */
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
        /**
         * \brief Property für die Startzeit des täglichen Zeitfilters.
         * 
         * Bestimmt die Startzeit innerhalb eines Tages für die Filterung.
         * 
         * \return Die Startzeit als TimeSpan.
         */
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
        /**
         * \brief Property für die Endzeit des täglichen Zeitfilters.
         * 
         * Bestimmt die Endzeit innerhalb eines Tages für die Filterung.
         * 
         * \return Die Endzeit als TimeSpan.
         */
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

        /**
         * \brief Collection aller verfügbaren Kinder.
         * 
         * \return ObservableCollection mit den Namen aller Kinder.
         */
        public ObservableCollection<string> Children { get; }

        /**
         * \brief Collection aller verfügbaren Symmetrie-Optionen.
         * 
         * \return ObservableCollection mit den verfügbaren Symmetrie-Filtern.
         */
        public ObservableCollection<string> SymmetryOptions { get; }

        /**
         * \brief Collection aller verfügbaren Chart-Typen.
         * 
         * \return ObservableCollection mit den verfügbaren Chart-Darstellungsarten.
         */
        public ObservableCollection<string> ChartTypes { get; }

        /**
         * \brief Collection der Symmetrie-Statistiken.
         * 
         * \return ObservableCollection mit aggregierten Statistiken pro Symmetrie-Typ.
         */
        public ObservableCollection<SymmetryStats> SymmetryStatsData { get; }

        /**
         * \brief Collection aller gefilterten Trainingseinheiten.
         * 
         * \return ObservableCollection mit den aktuell angezeigten Trainings.
         */
        public ObservableCollection<Training> Trainings { get; }

        /**
         * \brief Collection der Peer-Vergleichsdaten.
         * 
         * \return ObservableCollection mit Vergleichsdaten zwischen ausgewähltem Kind und Peers.
         */
        public ObservableCollection<PeerComparisonData> PeerComparisonData { get; }

        /**
         * \brief Collection der Peer-Symmetrie-Vergleichsdaten.
         * 
         * \return ObservableCollection mit symmetriespezifischen Peer-Vergleichen.
         */
        public ObservableCollection<PeerSymmetryComparison> PeerSymmetryComparison { get; }

        private int _totalSessions;
        /**
         * \brief Property für die Gesamtanzahl der Trainingssitzungen.
         * 
         * \return Die Anzahl der gefilterten Trainingssitzungen.
         */
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
        /**
         * \brief Property für die durchschnittliche Trainingszeit.
         * 
         * \return Die durchschnittliche Zeit in Sekunden über alle gefilterten Trainings.
         */
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
        /**
         * \brief Property für die durchschnittliche Fehleranzahl.
         * 
         * \return Die durchschnittliche Anzahl der Fehler über alle gefilterten Trainings.
         */
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
        /**
         * \brief Property für die beste (kürzeste) Trainingszeit.
         * 
         * \return Die beste Zeit in Sekunden aus allen gefilterten Trainings.
         */
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

        private double _peerAverageTime;
        /**
         * \brief Property für die durchschnittliche Trainingszeit der Peer-Gruppe.
         * 
         * \return Die durchschnittliche Zeit der Peers in Sekunden.
         */
        public double PeerAverageTime
        {
            get => _peerAverageTime;
            set
            {
                if (_peerAverageTime != value)
                {
                    _peerAverageTime = value;
                    OnPropertyChanged(nameof(PeerAverageTime));
                }
            }
        }

        private double _peerAverageErrors;
        /**
         * \brief Property für die durchschnittliche Fehleranzahl der Peer-Gruppe.
         * 
         * \return Die durchschnittliche Anzahl der Fehler der Peers.
         */
        public double PeerAverageErrors
        {
            get => _peerAverageErrors;
            set
            {
                if (_peerAverageErrors != value)
                {
                    _peerAverageErrors = value;
                    OnPropertyChanged(nameof(PeerAverageErrors));
                }
            }
        }

        private int _peerTotalSessions;
        /**
         * \brief Property für die Gesamtanzahl der Peer-Trainingssitzungen.
         * 
         * \return Die Gesamtanzahl aller Trainings der Peer-Gruppe.
         */
        public int PeerTotalSessions
        {
            get => _peerTotalSessions;
            set
            {
                if (_peerTotalSessions != value)
                {
                    _peerTotalSessions = value;
                    OnPropertyChanged(nameof(PeerTotalSessions));
                }
            }
        }

        private double _averageSessionsPerPeer;
        /**
         * \brief Property für die durchschnittliche Anzahl Sitzungen pro Peer.
         * 
         * \return Die durchschnittliche Anzahl Trainings pro Kind in der Peer-Gruppe.
         */
        public double AverageSessionsPerPeer
        {
            get => _averageSessionsPerPeer;
            set
            {
                if (_averageSessionsPerPeer != value)
                {
                    _averageSessionsPerPeer = value;
                    OnPropertyChanged(nameof(AverageSessionsPerPeer));
                }
            }
        }

        private int _peerCount;
        /**
         * \brief Property für die Anzahl der Kinder in der Peer-Gruppe.
         * 
         * \return Die Anzahl der Peers (Kinder des gleichen Jahrgangs).
         */
        public int PeerCount
        {
            get => _peerCount;
            set
            {
                if (_peerCount != value)
                {
                    _peerCount = value;
                    OnPropertyChanged(nameof(PeerCount));
                }
            }
        }

        private bool _isLoading;
        /**
         * \brief Property für den Ladezustand der Daten.
         * 
         * Zeigt an, ob gerade Daten geladen werden.
         * 
         * \return True wenn Daten geladen werden, sonst false.
         */
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

        /**
         * \brief Wendet alle aktuellen Filter auf die Trainingsdaten an.
         * 
         * Filtert die Daten nach Kind, Symmetrie, Datum und Tageszeit,
         * aktualisiert alle Statistiken und Chart-Daten.
         */
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

                await UpdatePeerComparisonAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Anwenden der Filter: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /**
         * \brief Berechnet und aktualisiert die grundlegenden Statistiken.
         * 
         * Berechnet Gesamtsitzungen, Durchschnittszeiten, Durchschnittsfehler
         * und beste Zeit aus den gefilterten Daten.
         * 
         * \param data Liste der gefilterten Trainingsdaten.
         */
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

        /**
         * \brief Aktualisiert die Peer-Vergleichsdaten.
         * 
         * Berechnet Statistiken für Kinder des gleichen Jahrgangs (Peers)
         * und erstellt Vergleichsdaten zwischen dem ausgewählten Kind und den Peers.
         */
        private async Task UpdatePeerComparisonAsync()
        {
            if (string.IsNullOrEmpty(SelectedChild))
                return;

            try
            {
                using var context = new PatternPixDbContext();

                var selectedChildData = await context.Children
                    .Where(c => (c.FirstName + " " + c.LastName) == SelectedChild)
                    .FirstOrDefaultAsync();

                if (selectedChildData == null)
                    return;

                int selectedChildBirthYear = selectedChildData.DateOfBirth.Year;

                var peerQuery = context.Trainings
                    .Include(t => t.Child)
                    .Where(t => (t.Child.FirstName + " " + t.Child.LastName) != SelectedChild &&
                               t.Child.DateOfBirth.Year == selectedChildBirthYear);

                if (SelectedSymmetry != "All")
                {
                    peerQuery = peerQuery.Where(t => t.Symmetry == SelectedSymmetry);
                }

                peerQuery = peerQuery.Where(t =>
                    t.Date.Date >= FromDate.Date &&
                    t.Date.Date <= ToDate.Date &&
                    t.TimeOfDay >= FromTime &&
                    t.TimeOfDay <= ToTime);

                var peerData = await peerQuery.ToListAsync();

                await Task.Run(() =>
                {
                    var peerChildren = peerData.Select(t => t.Child.FirstName + " " + t.Child.LastName).Distinct().ToList();
                    var peerSessionsPerChild = peerChildren.Select(peerName =>
                        peerData.Count(t => (t.Child.FirstName + " " + t.Child.LastName) == peerName)).ToList();

                    PeerCount = peerChildren.Count;
                    AverageSessionsPerPeer = peerSessionsPerChild.Any() ? peerSessionsPerChild.Average() : 0;
                    PeerTotalSessions = peerData.Count;
                    PeerAverageTime = peerData.Any() ? peerData.Average(x => x.TimeNeeded) : 0;
                    PeerAverageErrors = peerData.Any() ? peerData.Average(x => x.Errors) : 0;

                    var comparisonData = new List<PeerComparisonData>
                    {
                        new PeerComparisonData
                        {
                            Metric = "Average Time",
                            ChildValue = AverageTime,
                            PeerAverage = PeerAverageTime,
                            ChildName = SelectedChild
                        },
                        new PeerComparisonData
                        {
                            Metric = "Average Errors",
                            ChildValue = AverageErrors,
                            PeerAverage = PeerAverageErrors,
                            ChildName = SelectedChild
                        },
                        new PeerComparisonData
                        {
                            Metric = "Sessions",
                            ChildValue = TotalSessions,
                            PeerAverage = AverageSessionsPerPeer,
                            ChildName = SelectedChild
                        }
                    };

                    var peerSymmetryStats = peerData.GroupBy(x => x.Symmetry)
                        .Select(g => new { Symmetry = g.Key, AvgTime = g.Average(x => x.TimeNeeded), AvgErrors = g.Average(x => x.Errors) })
                        .ToDictionary(x => x.Symmetry, x => new { x.AvgTime, x.AvgErrors });

                    var symmetryComparison = SymmetryStatsData.Select(childStat => new PeerSymmetryComparison
                    {
                        Symmetry = childStat.Symmetry,
                        AvgTime = childStat.AvgTime,
                        AvgErrors = childStat.AvgErrors,
                        PeerAvgTime = peerSymmetryStats.ContainsKey(childStat.Symmetry) ?
                            peerSymmetryStats[childStat.Symmetry].AvgTime : 0,
                        PeerAvgErrors = peerSymmetryStats.ContainsKey(childStat.Symmetry) ?
                            peerSymmetryStats[childStat.Symmetry].AvgErrors : 0
                    }).ToList();

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        PeerComparisonData.Clear();
                        foreach (var comparison in comparisonData)
                        {
                            PeerComparisonData.Add(comparison);
                        }

                        PeerSymmetryComparison.Clear();
                        foreach (var comparison in symmetryComparison)
                        {
                            PeerSymmetryComparison.Add(comparison);
                        }

                        OnPropertyChanged(nameof(PeerComparisonData));
                        OnPropertyChanged(nameof(PeerSymmetryComparison));
                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating peer comparison: {ex.Message}");
            }
        }

        /**
         * \brief Lädt die Daten manuell neu.
         * 
         * Lädt sowohl die Kinderliste als auch alle gefilterten Daten neu.
         */
        public async Task RefreshDataAsync()
        {
            await LoadChildrenAsync();
            await ApplyFilterAsync();
        }

        /**
         * \brief Alias-Property für das Startdatum.
         * 
         * \return Das Startdatum für die Filterung.
         */
        public DateTime StartDate
        {
            get => FromDate;
            set => FromDate = value;
        }

        /**
         * \brief Alias-Property für das Enddatum.
         * 
         * \return Das Enddatum für die Filterung.
         */
        public DateTime EndDate
        {
            get => ToDate;
            set => ToDate = value;
        }

        private ObservableCollection<string> _timeRanges;
        /**
         * \brief Collection der verfügbaren Zeitraum-Optionen.
         * 
         * \return ObservableCollection mit vorgefertigten Zeitraum-Filtern.
         */
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
        /**
         * \brief Property für den ausgewählten Zeitraum.
         * 
         * Aktualisiert automatisch die FromTime und ToTime Properties
         * basierend auf der Auswahl.
         * 
         * \return Der ausgewählte Zeitraum als String.
         */
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

        /**
         * \brief Alias-Property für die gefilterten Trainings.
         * 
         * \return Die aktuell gefilterte Trainings-Collection.
         */
        public ObservableCollection<Training> FilteredTrainings => Trainings;

        /**
         * \brief Property für den Titel des primären Charts.
         * 
         * Der Titel ändert sich basierend auf dem ausgewählten Chart-Typ.
         * 
         * \return Der Chart-Titel als String.
         */
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

        /**
         * \brief Property für die Beschriftung der primären Y-Achse.
         * 
         * Die Beschriftung ändert sich basierend auf dem ausgewählten Chart-Typ.
         * 
         * \return Die Y-Achsen-Beschriftung als String.
         */
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

        /**
         * \brief Property für den Datenpfad der primären Chart-Serie.
         * 
         * Bestimmt welche Eigenschaft der Training-Objekte für die primäre Serie verwendet wird.
         * 
         * \return Der Eigenschaftsname als String.
         */
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

        /**
         * \brief Property für den Datenpfad der sekundären Chart-Serie.
         * 
         * Bestimmt welche Eigenschaft für die sekundäre Serie verwendet wird (falls vorhanden).
         * 
         * \return Der Eigenschaftsname als String oder null.
         */
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

        /**
         * \brief Property für die Beschriftung der primären Chart-Serie.
         * 
         * \return Die Beschriftung der primären Serie als String.
         */
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

        /**
         * \brief Property für die Beschriftung der sekundären Chart-Serie.
         * 
         * \return Die Beschriftung der sekundären Serie als String.
         */
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

        /**
          * \brief Property für die Sichtbarkeit der sekundären Chart-Serie.
          * 
          * \return Visibility.Visible wenn eine sekundäre Serie angezeigt werden soll, sonst Visibility.Collapsed.
          */
        public Visibility ShowSecondarySeriesVisibility
        {
            get => SelectedChartType == "Errors/Time" ? Visibility.Visible : Visibility.Collapsed;
        }

        /**
         * \brief Property für die Symmetrie-Statistiken als neue Collection.
         * 
         * Erstellt eine neue ObservableCollection basierend auf den aktuellen SymmetryStatsData.
         * 
         * \return Eine neue ObservableCollection mit Symmetrie-Statistiken.
         */
        public ObservableCollection<SymmetryStats> SymmetryStats =>
            new ObservableCollection<SymmetryStats>(SymmetryStatsData.Select(s => new SymmetryStats
            {
                Symmetry = s.Symmetry,
                AvgTime = s.AvgTime,
                AvgErrors = s.AvgErrors
            }));

        /**
         * \brief ObservableCollection für die Datenpunkte des gleitenden Durchschnitts.
         * 
         * Diese Collection enthält die berechneten Trendpunkte für die Visualisierung
         * des gleitenden Durchschnitts in der Chart-Ansicht.
         */
        public ObservableCollection<MovingAveragePoint> MovingAverageData { get; } = new();

        /**
         * \brief Aktualisiert die Zeitbereich-Filter basierend auf der ausgewählten Zeitspanne.
         * 
         * Setzt die FromTime und ToTime Properties entsprechend der gewählten Tageszeit.
         * Unterstützt Morgen, Nachmittag, Abend oder den gesamten Tag.
         * 
         * \param timeRange Der ausgewählte Zeitbereich als String.
         */
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

        /**
         * \brief Aktualisiert die Chart-Daten asynchron.
         * 
         * Ersetzt die aktuelle Trainings-Collection mit neuen Daten und
         * benachrichtigt alle abhängigen Properties über Änderungen.
         * Die Daten werden nach Datum und Tageszeit sortiert.
         * 
         * \param data Liste der neuen Training-Objekte.
         * \return Task für die asynchrone Ausführung.
         */
        private async Task UpdateChartDataAsync(List<Training> data)
        {
            await Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Trainings.Clear();
                    foreach (var training in data.OrderBy(t => t.Date).ThenBy(t => t.TimeOfDay))
                    {
                        Trainings.Add(training);
                    }

                    OnPropertyChanged(nameof(SymmetryStats));
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

        /**
         * \brief Berechnet und aktualisiert die Daten für den gleitenden Durchschnitt asynchron.
         * 
         * Verwendet ein Fenster von 5 Datenpunkten zur Berechnung des gleitenden Durchschnitts
         * für Fehler und benötigte Zeit. Die Ergebnisse werden in der MovingAverageData-Collection gespeichert.
         * 
         * \param data Liste der Training-Objekte für die Berechnung.
         * \return Task für die asynchrone Ausführung.
         */
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

        /**
         * \brief Berechnet und aktualisiert die Symmetrie-Statistiken asynchron.
         * 
         * Gruppiert die Trainingsdaten nach Symmetrie-Typ und berechnet die durchschnittliche
         * Zeit und Fehleranzahl für jede Symmetrie. Die Ergebnisse werden in der
         * SymmetryStatsData-Collection gespeichert.
         * 
         * \param data Liste der Training-Objekte für die Statistik-Berechnung.
         * \return Task für die asynchrone Ausführung.
         */
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