using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Command;
using System.Windows.Input;
using PatternPixTrainerPanel.Events;
using PatternPixTrainerPanel.Model;
using System.Collections.ObjectModel;
using Microsoft.Identity.Client;
using Prism.Events;

namespace PatternPixTrainerPanel.ViewModel
{
    public class ChildAddViewModel : BaseViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public ChildAddViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private ICommand _backCommand;
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
