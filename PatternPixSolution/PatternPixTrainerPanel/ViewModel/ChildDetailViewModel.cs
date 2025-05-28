using PatternPixTrainerPanel.Model;
using PatternPixTrainerPanel.Events;
using PatternPixTrainerPanel.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Common.Command;
using Prism.Events;

namespace PatternPixTrainerPanel.ViewModel
{
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

        /// \brief Instanz des EventAggregators zur Eventkommunikation.
        private readonly IEventAggregator _eventAggregator;

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
                OnPropertyChanged("Trainings");
            }
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
                        .OrderBy(t => t.Date) // Chronologisch sortieren für bessere Visualisierung
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