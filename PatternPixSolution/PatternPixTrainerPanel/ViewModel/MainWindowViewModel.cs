using PatternPixTrainerPanel.Events;
using PatternPixTrainerPanel.View;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace PatternPixTrainerPanel.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly Dictionary<string, UserControl> _views;
        private readonly IEventAggregator _eventAggregator;
        private UserControl _currentView;

        public MainWindowViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

            // Subscribe to navigation events
            _eventAggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigate);

            // Initialize views dictionary
            _views = new Dictionary<string, UserControl>();
        }

        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public void RegisterView(string viewName, UserControl view)
        {
            if (string.IsNullOrWhiteSpace(viewName))
                throw new ArgumentException("View name cannot be empty", nameof(viewName));

            if (view == null)
                throw new ArgumentNullException(nameof(view));

            _views[viewName] = view;

            // Set the first registered view as the current view
            if (CurrentView == null)
                CurrentView = view;
        }

        private void OnNavigate(string viewName)
        {
            if (_views.ContainsKey(viewName))
            {
                CurrentView = _views[viewName];
            }
        }
    }
}