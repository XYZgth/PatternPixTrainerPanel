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
    /**
     * \brief ViewModel für die Ansicht zum Hinzufügen eines Kindes.
     *
     * Diese Klasse enthält die Logik zur Navigation zurück zur Hauptansicht und dient als Bindeglied zwischen View und Daten.
     */
    public class ChildAddViewModel : BaseViewModel
    {
        /// \brief Instanz des EventAggregators zur Eventkommunikation.
        private readonly IEventAggregator _eventAggregator;

        /**
         * \brief Konstruktor für das ChildAddViewModel.
         *
         * Initialisiert das ViewModel mit dem übergebenen EventAggregator.
         *
         * \param eventAggregator Eine gültige Instanz des Prism EventAggregator.
         */
        public ChildAddViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// \brief Befehl zum Zurücknavigieren zur Hauptansicht.
        private ICommand _backCommand;

        /**
         * \brief Property für den BackCommand.
         *
         * Dieser Befehl navigiert zur "MainView", wenn er ausgeführt wird.
         * 
         * \return Ein ICommand, das bei Ausführung ein Navigationsevent publiziert.
         */
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