using PatternPixTrainerPanel.Events;
using PatternPixTrainerPanel.View;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PatternPixTrainerPanel.ViewModel
{
    /**
     * \brief ViewModel für das Hauptfenster.
     * 
     * Dieses ViewModel verwaltet die Navigation zwischen verschiedenen Views
     * und reagiert auf Navigationsevents über den EventAggregator.
     */
    public class MainWindowViewModel : BaseViewModel
    {
        /// \brief Dictionary zur Speicherung registrierter Views anhand ihres Namens.
        private readonly Dictionary<string, UserControl> _views;

        /// \brief Der EventAggregator zur Event-Kommunikation.
        private readonly IEventAggregator _eventAggregator;

        /// \brief Die aktuell angezeigte View.
        private UserControl _currentView;

        /**
         * \brief Konstruktor initialisiert EventAggregator und Abonnement auf Navigationsevents.
         * 
         * \param eventAggregator Instanz des EventAggregators zur Event-Kommunikation.
         */
        public MainWindowViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // Navigationsevent abonnieren
            _eventAggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigate);

            // View-Wörterbuch initialisieren
            _views = new Dictionary<string, UserControl>();
        }

        /**
         * \brief Aktuell angezeigte Benutzersteuerung (View).
         */
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        /**
         * \brief Registriert eine neue View mit zugehörigem Namen.
         * 
         * \param viewName Name der View (als Schlüssel im Dictionary).
         * \param view Die View, die registriert werden soll.
         * 
         * \throws ArgumentException Wenn der Name leer oder null ist.
         * \throws ArgumentNullException Wenn die View null ist.
         */
        public void RegisterView(string viewName, UserControl view)
        {
            if (string.IsNullOrWhiteSpace(viewName))
                throw new ArgumentException("View name cannot be empty", nameof(viewName));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            _views[viewName] = view;

            // Die erste registrierte View als aktuelle setzen
            if (CurrentView == null)
                CurrentView = view;
        }

        /**
         * \brief Reagiert auf ein Navigationsevent und wechselt zur entsprechenden View.
         * 
         * \param viewName Der Name der View, zu der navigiert werden soll.
         */
        private void OnNavigate(string viewName)
        {
            if (_views.ContainsKey(viewName))
            {
                CurrentView = _views[viewName];
            }
        }
    }
}
