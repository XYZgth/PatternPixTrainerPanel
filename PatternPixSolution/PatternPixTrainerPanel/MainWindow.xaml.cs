using System.Windows;
using PatternPixTrainerPanel.ViewModel;
using PatternPixTrainerPanel.View;
using PatternPixTrainerPanel.Converter;
using Prism.Events;
using System.Windows.Controls;
using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly IEventAggregator _eventAggregator;

        public MainWindow()
        {
            InitializeComponent();

            var fileRepo = new FileChildRepository();


            _eventAggregator = new EventAggregator();

          
            _mainViewModel = new MainWindowViewModel(_eventAggregator);

          
            DataContext = _mainViewModel;

      
      
            RegisterViews();
        }

        private void RegisterViews()
        {
            
            var mainView = new MainView();
            mainView.DataContext = new ChildrenListViewModel(_eventAggregator);
            _mainViewModel.RegisterView("MainView", mainView);


            var childDetailView = new ChildDetailView();
            childDetailView.DataContext = new ChildDetailViewModel(_eventAggregator);
            _mainViewModel.RegisterView("ChildDetailView", childDetailView);

            var childAddView = new ChildAddView();
            childAddView.DataContext = new ChildAddViewModel(_eventAggregator);
            _mainViewModel.RegisterView("ChildAddView", childAddView);

        }
    }
}