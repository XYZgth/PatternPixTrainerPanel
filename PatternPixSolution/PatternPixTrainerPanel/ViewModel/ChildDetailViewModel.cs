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
    public class ChildDetailViewModel : BaseViewModel
    {
        private Child _selectedChild;
        private ObservableCollection<Training> _trainings;
        private readonly IEventAggregator _eventAggregator;

        public ChildDetailViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;

            
            _eventAggregator.GetEvent<ChildSelectedEvent>().Subscribe(OnChildSelected);

            Trainings = new ObservableCollection<Training>();
        }

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

        public string FullName => SelectedChild?.FullName ?? "No Child Selected";
        public int? Age => SelectedChild?.Age;
        public DateTime? DateOfBirth => SelectedChild?.DateOfBirth;

        public ObservableCollection<Training> Trainings
        {
            get { return _trainings; }
            set
            {
                _trainings = value;
                OnPropertyChanged("Trainings");
            }
        }

        private void OnChildSelected(Child child)
        {
            SelectedChild = child;
        }

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
                        .OrderByDescending(t => t.Date)
                        .ThenByDescending(t => t.TimeOfDay)
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