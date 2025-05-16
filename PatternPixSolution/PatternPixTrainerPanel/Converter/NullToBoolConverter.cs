using System;
using System.Globalization;
using System.Windows.Data;

namespace PatternPixTrainerPanel.Converter
{
    /**
     * \brief Konvertiert einen Wert von null auf bool.
     * 
     * Diese Klasse implementiert IValueConverter und gibt true zurück, wenn der Wert nicht null ist,
     * andernfalls false.
     */
    public class NullToBoolConverter : IValueConverter
    {
        /**
         * \brief Konvertiert einen Wert von null auf bool.
         * 
         * \param value Der Eingabewert, der geprüft wird.
         * \param targetType Der Zieltyp der Konvertierung.
         * \param parameter Optionaler Parameter für die Konvertierung.
         * \param culture Die Kulturinformation.
         * \return true, wenn value nicht null ist, sonst false.
         */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Gibt true zurück, wenn der Wert nicht null ist
            return value != null;
        }

        /**
         * \brief Nicht implementierte Rückwärtskonvertierung.
         * 
         * \throws NotImplementedException Diese Methode ist nicht implementiert.
         */
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
