using Common;
using Prism.Events;
using System;

namespace PatternPixTrainerPanel.ViewModel
{
    /**
     * \brief Basisklasse für alle ViewModels im MVVM-Pattern.
     *
     * Diese Klasse stellt die gemeinsame Funktionalität für ViewModels bereit,
     * insbesondere Zugriff auf den EventAggregator von Prism für die Kommunikation zwischen Komponenten.
     */
    public class BaseViewModel : NotifyPropertyChanged
    {
        /// \brief Instanz des EventAggregators zur Eventkommunikation.
        protected IEventAggregator EventAggregator { get; }

        /**
         * \brief Konstruktor für das BaseViewModel.
         *
         * Initialisiert den EventAggregator, der für die lose Kopplung der Komponenten sorgt.
         *
         * \param eventAggregator Eine gültige Instanz des Prism EventAggregator.
         * \throws ArgumentNullException Wenn der eventAggregator null ist.
         */
        public BaseViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
        }
    }
}