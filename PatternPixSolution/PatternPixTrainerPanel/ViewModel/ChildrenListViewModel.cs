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
    public class ChildrenListViewModel : BaseViewModel
    {
        private ObservableCollection<Child> _children;
        private Child _selectedChild;
        private readonly IEventAggregator _eventAggregator;

        public ChildrenListViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

           
            LoadChildren();
        }

        public ObservableCollection<Child> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged("Children");
            }
        }

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

        
        private ICommand _refreshCommand;
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

        
        private ICommand _viewDetailsCommand;
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

        private ICommand _viewaddcommand;
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
    }
}