using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternPixTrainerPanel.ViewModel
{

    public class BaseViewModel : NotifyPropertyChanged
    {
        protected IEventAggregator EventAggregator { get; }

        public BaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(EventAggregator));
        }
    }
}