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
using PatternPixTrainerPanel.Repositories;
using System.Windows;

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

        /// \brief Gefilterte Sammlung der Kinder basierend auf Suchtext.
        private ObservableCollection<Child> _filteredChildren;

        /// \brief Suchtext für die Filterung der Kinderliste.
        private string _searchText = string.Empty;

        /// \brief Aktuell ausgewähltes Kind.
        private Child _selectedChild;

        private int _selectedRepositoryMode;

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
            _filteredChildren = new ObservableCollection<Child>();
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
                ApplyFilter();
            }
        }

        /**
         * \brief Gefilterte Liste der Kinder basierend auf dem Suchtext.
         * Diese Collection wird an das DataGrid gebunden.
         */
        public ObservableCollection<Child> FilteredChildren
        {
            get { return _filteredChildren; }
            set
            {
                _filteredChildren = value;
                OnPropertyChanged("FilteredChildren");
            }
        }

        /**
         * \brief Suchtext für die Filterung der Kinderliste.
         * Änderungen führen automatisch zur Neufilterung.
         */
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                ApplyFilter();
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
                var children = App.ChildRepository?.LoadChildren() ?? new List<Child>();
                Children = new ObservableCollection<Child>(children);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading children: {ex.Message}");
                Children = new ObservableCollection<Child>();
            }
        }

        /**
         * \brief Wendet den aktuellen Suchfilter auf die Kinderliste an.
         * Filtert nach Vor- und Nachname des Kindes.
         */
        private void ApplyFilter()
        {
            if (Children == null)
            {
                FilteredChildren?.Clear();
                return;
            }

            FilteredChildren.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Children
                : Children.Where(child =>
                    $"{child.FirstName} {child.LastName}"
                    .Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var child in filtered)
            {
                FilteredChildren.Add(child);
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

        /**
         * \brief Befehl zum Öffnen der Analyseansicht.
         * 
         * \return Ein ICommand zur Navigation zur Analyseansicht.
         */
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

        /**
         * \brief Aktuell ausgewählter Repository-Modus (z. B. DB oder Datei).
         * 
         * Änderung führt zum Neuladen der Kinderliste mit dem neuen Modus.
         */
        public int SelectedRepositoryMode
        {
            get => _selectedRepositoryMode;
            set
            {
                if (_selectedRepositoryMode != value)
                {
                    _selectedRepositoryMode = value;

                    App.SetRepositoryMode(_selectedRepositoryMode);
                    LoadChildren();
                }
            }
        }
    }
}