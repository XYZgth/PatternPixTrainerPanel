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
using PatternPixTrainerPanel.Repositories;
using System.Windows;

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
            DateOfBirth = DateTime.Now.AddYears(-10);
            _eventAggregator = eventAggregator;
        }

        private string _childName;

        /**
         * \brief Vorname des Kindes.
         */
        public string ChildName
        {
            get => _childName;
            set
            {
                _childName = value;
                OnPropertyChanged(nameof(ChildName));
            }
        }
        private string _lastName;

        /**
         * \brief Nachname des Kindes.
         */
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private DateTime _dateOfBirth;

        /**
         * \brief Geburtsdatum des Kindes.
         */
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        /// \brief Befehl zum Zurücknavigieren zur Hauptansicht.
        private ICommand _backCommand;

        /// \brief Befehl zum Hinzufügen eines neuen Kindes.
        private ICommand _addCommand;

        /// \brief Befehl zum Abbrechen der Eingabe und Zurücksetzen der Felder.
        private ICommand _cancelCommand;

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

        /**
         * \brief Property für den AddCommand.
         *
         * Fügt ein neues Kind mit den eingegebenen Daten hinzu, sofern die Felder korrekt ausgefüllt sind.
         * Zeigt bei fehlenden Eingaben eine Fehlermeldung an.
         * 
         * \return Ein ICommand zur Ausführung des Hinzufügens.
         */
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new ActionCommand(
                        param =>
                        {
                            if (string.IsNullOrWhiteSpace(ChildName) || string.IsNullOrWhiteSpace(LastName))
                            {
                                MessageBox.Show("Bitte Vor- und Nachname eingeben.", "Fehler", MessageBoxButton.OK);
                                return;
                            }

                            // Neues Kind-Objekt erstellen
                            var newChild = new Child
                            {
                                FirstName = this.ChildName,
                                LastName = this.LastName,
                                DateOfBirth = this.DateOfBirth
                            };
                            IChildRepository _childRepository = PatternPixTrainerPanel.App.ChildRepository;
                            
                            // Speichern
                            _childRepository.SaveChildren(new List<Child> { newChild });

                            // Feld clear
                            ChildName = string.Empty;
                            LastName = string.Empty;
                            DateOfBirth = DateTime.Now.AddYears(-10);
                        },
                        param => true
                    );
                }
                return _addCommand;
            }
        }

        /**
         * \brief Property für den CancelCommand.
         *
         * Setzt alle Eingabefelder zurück.
         * 
         * \return Ein ICommand zur Ausführung der Zurücksetzung.
         */
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new ActionCommand(
                        param =>
                        {
                            ChildName = string.Empty;
                            LastName = string.Empty;
                            DateOfBirth = DateTime.Now.AddYears(-10);
                        },
                        param => true
                    );
                }
                return _cancelCommand;
            }
        }
    }
}