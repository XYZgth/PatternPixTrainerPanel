using System.Windows;
using PatternPixTrainerPanel.ViewModel;
using PatternPixTrainerPanel.View;
using PatternPixTrainerPanel.Converter;
using Prism.Events;
using System.Windows.Controls;
using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;
using Microsoft.EntityFrameworkCore;

namespace PatternPixTrainerPanel
{
    /**
     * \brief Hauptfenster der Anwendung.
     *
     * Diese Klasse initialisiert das Hauptfenster, setzt das DataContext und registriert die verwendeten Views gemäß dem MVVM-Muster.
     */
    public partial class MainWindow : Window
    {
        /// \brief Das Haupt-ViewModel der Anwendung.
        private readonly MainWindowViewModel _mainViewModel;

        /// \brief EventAggregator von Prism zur Kommunikation zwischen ViewModels.
        private readonly IEventAggregator _eventAggregator;

       

        /**
         * \brief Konstruktor des Hauptfensters.
         *
         * Initialisiert die grafische Oberfläche, den EventAggregator und das Haupt-ViewModel.
         * Danach werden die Views registriert.
         */
        public MainWindow()
        {
            InitializeComponent();

            var fileRepo = new FileChildRepository();

            _eventAggregator = new EventAggregator();

            _mainViewModel = new MainWindowViewModel(_eventAggregator);
         

            DataContext = _mainViewModel;

            RegisterViews();
        }

        /**
         * \brief Registriert alle benötigten Views in der Anwendung.
         *
         * Diese Methode erstellt Instanzen der Views sowie deren zugehörige ViewModels
         * und registriert sie im Haupt-ViewModel zur Navigation.
         */
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

            var analysisView = new AnalysisView();
            analysisView.DataContext = new AnalysisViewModel(_eventAggregator);
            _mainViewModel.RegisterView("AnalysisView", analysisView);
        }
    }
}
