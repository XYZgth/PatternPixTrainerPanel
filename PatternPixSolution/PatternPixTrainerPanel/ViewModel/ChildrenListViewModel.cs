using PatternPixTrainerPanel.Model;
using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Events;
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
     * \brief ViewModel zur Anzeige und Verwaltung der Liste aller Kinder.
     * 
     * Ermöglicht das Laden, Auswählen und Navigieren zu Details oder Hinzufügen eines Kindes.
     */
    public class ChildrenListViewModel : BaseViewModel
    {
        /// \brief Sammlung aller Kinder.
        private ObservableCollection<Child> _children;

        /// \brief Aktuell ausgewähltes Kind.
        private Child _selectedChild;

        /// \brief EventAggregator zur Kommunikation zwischen Komponenten.
        private readonly IEventAggregator _eventAggregator;

        /**
         * \brief Konstruktor, lädt beim Start alle Kinder aus der Datenbank.
         * 
         * \param eventAggregator EventAggregator zur Eventkommunikation.
         */
        public ChildrenListViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
            LoadChildren();
        }

        /**
         * \brief Liste aller Kinder.
         */
        public ObservableCollection<Child> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }

        /**
         * \brief Das aktuell ausgewählte Kind.
         * 
         * Löst ChildSelectedEvent aus, wenn ein Kind ausgewählt wurde.
         */
        public Child SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                OnPropertyChanged("SelectedChild");

                if (_selectedChild != null)
                {
                    _eventAggregator.GetEvent<ChildSelectedEvent>().Publish(_selectedChild);
                }
            }
        }

        /**
         * \brief Lädt alle Kinder samt zugehöriger Trainingsdaten aus der Datenbank.
         */
        private void LoadChildren()
        {
            try
            {
                using (var context = new PatternPixDbContext())
                {
                    var childrenList = context.Children
                        .Include(c => c.Trainings)
                        .ToList();

                    Children = new ObservableCollection<Child>(childrenList);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading children: {ex.Message}");
                Children = new ObservableCollection<Child>();
            }
        }

        /// \brief Befehl zum Aktualisieren der Kindliste.
        private ICommand _refreshCommand;

        /**
         * \brief Befehl zum manuellen Neuladen der Kinderliste.
         */
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new ActionCommand(
                        param => LoadChildren(),
                        param => true);
                }
                return _refreshCommand;
            }
        }

        /// \brief Befehl zum Wechseln zur Detailansicht eines ausgewählten Kindes.
        private ICommand _viewDetailsCommand;

        /**
         * \brief Navigiert zur Detailansicht des aktuell ausgewählten Kindes.
         * 
         * Voraussetzung: Es wurde ein Kind ausgewählt.
         */
        public ICommand ViewDetailsCommand
        {
            get
            {
                if (_viewDetailsCommand == null)
                {
                    _viewDetailsCommand = new ActionCommand(
                        param => {
                            _eventAggregator.GetEvent<ChildSelectedEvent>().Publish(SelectedChild);
                            _eventAggregator.GetEvent<NavigationEvent>().Publish("ChildDetailView");
                        },
                        param => SelectedChild != null);
                }
                return _viewDetailsCommand;
            }
        }

        /// \brief Befehl zum Wechseln zur Ansicht zum Hinzufügen eines Kindes.
        private ICommand _viewaddcommand;

        /**
         * \brief Navigiert zur Ansicht zum Hinzufügen eines neuen Kindes.
         */
        public ICommand ViewAddCommand
        {
            get
            {
                if (_viewaddcommand == null)
                {
                    _viewaddcommand = new ActionCommand(
                        param => _eventAggregator.GetEvent<NavigationEvent>().Publish("ChildAddView"),
                        param => true);
                }
                return _viewaddcommand;
            }
        }
        private ICommand _analysisCommand;
        public ICommand AnalysisViewCommand
        {
            get
            {
                if (_analysisCommand == null)
                {
                    _analysisCommand = new ActionCommand(
                        param => _eventAggregator.GetEvent<NavigationEvent>().Publish("AnalysisView"),
                        param => true);
                }
                return _analysisCommand;
            }
        }
    }
}
