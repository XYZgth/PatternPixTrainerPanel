using PatternPixTrainerPanel.Model;
using PatternPixTrainerPanel.Events;
using PatternPixTrainerPanel.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Common.Command;
using Prism.Events;
using System.Windows.Data;

namespace PatternPixTrainerPanel.ViewModel
{
    /// <summary>
    /// Represents a group of trainings within a specific time window
    /// </summary>
    public class TrainingTimeGroup
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeRangeDisplay => $"{StartTime:HH:mm} - {EndTime:HH:mm}";
        public ObservableCollection<Training> Trainings { get; set; } = new ObservableCollection<Training>();
    }

    /// <summary>
    /// Represents a day with its training time groups
    /// </summary>
    public class TrainingDayGroup
    {
        public DateTime Date { get; set; }
        public string DateDisplay => Date.ToString("dd.MM.yyyy");
        public ObservableCollection<TrainingTimeGroup> TimeGroups { get; set; } = new ObservableCollection<TrainingTimeGroup>();
    }

    /**
     * \brief ViewModel zur Anzeige der Details eines ausgewählten Kindes.
     * 
     * Dieses ViewModel verwaltet die Darstellung eines einzelnen Kindes und seiner Trainingsdaten.
     */
    public class ChildDetailViewModel : BaseViewModel
    {
        /// \brief Das aktuell ausgewählte Kind.
        private Child _selectedChild;

        /// \brief Liste der Trainings, die dem Kind zugeordnet sind.
        private ObservableCollection<Training> _trainings;

        /// \brief Gruppierte Trainings nach Datum und Zeitfenstern.
        private ObservableCollection<TrainingDayGroup> _groupedTrainingsByTime;

        /// \brief Instanz des EventAggregators zur Eventkommunikation.
        private readonly IEventAggregator _eventAggregator;

        /// \brief Zeitfenster in Minuten für die Gruppierung (Standard: 10 Minuten).
        private int _timeWindowMinutes = 10;

        /**
         * \brief Konstruktor für das ChildDetailViewModel.
         * 
         * Registriert sich für das ChildSelectedEvent und initialisiert die Trainingsliste.
         * 
         * \param eventAggregator Instanz des EventAggregators für Eventkommunikation.
         */
        public ChildDetailViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ChildSelectedEvent>().Subscribe(OnChildSelected);

            Trainings = new ObservableCollection<Training>();
            GroupedTrainingsByTime = new ObservableCollection<TrainingDayGroup>();
        }

        /**
         * \brief Property für das aktuell ausgewählte Kind.
         * 
         * Aktualisiert zusätzlich FullName, Age, DateOfBirth und lädt Trainingsdaten bei Änderung.
         */
        public Child SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                OnPropertyChanged("SelectedChild");
                OnPropertyChanged("FullName");
                OnPropertyChanged("Age");
                OnPropertyChanged("DateOfBirth");

                LoadTrainings();
            }
        }

        /**
         * \brief Vollständiger Name des ausgewählten Kindes.
         */
        public string FullName => SelectedChild?.FullName ?? "No Child Selected";

        /**
         * \brief Alter des ausgewählten Kindes.
         */
        public int? Age => SelectedChild?.Age;

        /**
         * \brief Geburtsdatum des ausgewählten Kindes.
         */
        public DateTime? DateOfBirth => SelectedChild?.DateOfBirth;

        /**
         * \brief Liste aller Trainings des Kindes.
         */
        public ObservableCollection<Training> Trainings
        {
            get { return _trainings; }
            set
            {
                _trainings = value;
                OnPropertyChanged(nameof(Trainings));

                UpdateGroupedTrainings();
                UpdateTimeBasedGroupings();
            }
        }

        /**
         * \brief Gruppierte Ansicht der Trainingsdaten nach Datum und Zeitfenstern.
         */
        public ObservableCollection<TrainingDayGroup> GroupedTrainingsByTime
        {
            get => _groupedTrainingsByTime;
            private set
            {
                _groupedTrainingsByTime = value;
                OnPropertyChanged(nameof(GroupedTrainingsByTime));
            }
        }

        
        

        // Keep the original grouped trainings for backward compatibility
        private ListCollectionView _groupedTrainings;
        public ListCollectionView GroupedTrainings
        {
            get => _groupedTrainings;
            private set
            {
                _groupedTrainings = value;
                OnPropertyChanged(nameof(GroupedTrainings));
            }
        }

        private void UpdateGroupedTrainings()
        {
            if (_trainings == null)
                return;

            var view = new ListCollectionView(_trainings);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Training.DateString)));
            GroupedTrainings = view;
        }

        /**
         * \brief Erstellt eine erweiterte Gruppierung nach Datum und Zeitfenstern.
         */
        private void UpdateTimeBasedGroupings()
        {
            if (_trainings == null || !_trainings.Any())
            {
                GroupedTrainingsByTime = new ObservableCollection<TrainingDayGroup>();
                return;
            }

            var dayGroups = new List<TrainingDayGroup>();

            // Gruppiere zunächst nach Datum
            var trainingsByDate = _trainings
                .GroupBy(t => t.Date.Date)
                .OrderBy(g => g.Key);

            foreach (var dayGroup in trainingsByDate)
            {
                var dayGroupObj = new TrainingDayGroup
                {
                    Date = dayGroup.Key
                };

                // Sortiere Trainings des Tages nach Zeit
                var sortedTrainings = dayGroup
                    .OrderBy(t => t.TimeOfDay)
                    .ToList();

                // Erstelle Zeitfenster-Gruppen
                var timeGroups = CreateTimeGroups(sortedTrainings);

                foreach (var timeGroup in timeGroups)
                {
                    dayGroupObj.TimeGroups.Add(timeGroup);
                }

                dayGroups.Add(dayGroupObj);
            }

            GroupedTrainingsByTime = new ObservableCollection<TrainingDayGroup>(dayGroups);
        }

        /**
         * \brief Erstellt Zeitfenster-Gruppen aus einer Liste von Trainings eines Tages.
         * 
         * \param trainings Sortierte Liste der Trainings eines Tages.
         * \return Liste der Zeitfenster-Gruppen.
         */
        private List<TrainingTimeGroup> CreateTimeGroups(List<Training> trainings)
        {
            var timeGroups = new List<TrainingTimeGroup>();

            if (!trainings.Any())
                return timeGroups;

            var currentGroup = new TrainingTimeGroup();
            var firstTraining = trainings.First();

            // Initialisiere die erste Gruppe
            currentGroup.StartTime = firstTraining.Date.Add(firstTraining.TimeOfDay);
            currentGroup.EndTime = currentGroup.StartTime;
            currentGroup.Trainings.Add(firstTraining);

            for (int i = 1; i < trainings.Count; i++)
            {
                var currentTraining = trainings[i];
                var currentTrainingTime = currentTraining.Date.Add(currentTraining.TimeOfDay);

                // Prüfe, ob das aktuelle Training in das Zeitfenster der aktuellen Gruppe passt
                var timeDifference = currentTrainingTime - currentGroup.EndTime;

                if (timeDifference.TotalMinutes <= 10)
                {
                    // Füge zur aktuellen Gruppe hinzu
                    currentGroup.Trainings.Add(currentTraining);
                    currentGroup.EndTime = currentTrainingTime;
                }
                else
                {
                    // Erstelle eine neue Gruppe
                    timeGroups.Add(currentGroup);

                    currentGroup = new TrainingTimeGroup
                    {
                        StartTime = currentTrainingTime,
                        EndTime = currentTrainingTime
                    };
                    currentGroup.Trainings.Add(currentTraining);
                }
            }

            // Füge die letzte Gruppe hinzu
            if (currentGroup.Trainings.Any())
            {
                timeGroups.Add(currentGroup);
            }

            return timeGroups;
        }

        /**
         * \brief Wird aufgerufen, wenn ein Kind über das ChildSelectedEvent ausgewählt wurde.
         * 
         * \param child Das ausgewählte Kind.
         */
        private void OnChildSelected(Child child)
        {
            SelectedChild = child;
        }

        /**
         * \brief Lädt alle Trainingsdaten für das aktuell ausgewählte Kind aus der Datenbank.
         */
        private void LoadTrainings()
        {
            if (SelectedChild == null)
            {
                Trainings.Clear();
                return;
            }

            try
            {
                using (var context = new PatternPixDbContext())
                {
                    var trainings = context.Trainings
                        .Where(t => t.ChildId == SelectedChild.Id)
                        .OrderBy(t => t.Date) // Chronologisch sortieren
                        .ThenBy(t => t.TimeOfDay)
                        .ToList();

                    Trainings = new ObservableCollection<Training>(trainings);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading trainings: {ex.Message}");
                Trainings = new ObservableCollection<Training>();
            }
        }

        /// \brief Befehl zur Navigation zurück zur Hauptansicht.
        private ICommand _backCommand;

        /**
         * \brief Property für den BackCommand.
         * 
         * Navigiert bei Ausführung zur "MainView".
         * 
         * \return Ein ICommand zur Navigation.
         */
        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new ActionCommand(
                        param => _eventAggregator.GetEvent<NavigationEvent>().Publish("MainView"),
                        param => true);
                }
                return _backCommand;
            }
        }
    }
}