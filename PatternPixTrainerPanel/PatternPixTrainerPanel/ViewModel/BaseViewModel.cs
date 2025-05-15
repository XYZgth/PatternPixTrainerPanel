using Common;
using Prism.Events;
using System;

namespace PatternPixTrainerPanel.ViewModel
{
    public class BaseViewModel : NotifyPropertyChanged
    {
        protected IEventAggregator EventAggregator { get; }

        public BaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }
    }
}