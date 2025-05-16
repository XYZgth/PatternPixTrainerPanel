using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PatternPixTrainerPanel.Model;
using System.IO;

namespace PatternPixTrainerPanel.Data
{
    /**
     * \brief Verwaltet das Laden und Speichern von Child-Daten in einer JSON-Datei.
     */
    public class FileChildRepository
    {
        /// \brief Pfad zur JSON-Datei, in der die Kinder-Daten gespeichert sind.
        private const string FilePath = "..\\..\\..\\..\\children.json";

        /**
         * \brief Lädt die Liste der Kinder aus der JSON-Datei.
         * 
         * Prüft zunächst, ob die Datei existiert. Falls nicht, wird eine leere Liste zurückgegeben.
         * 
         * \return Liste der geladenen Child-Objekte.
         */
        public List<Child> LoadChildren()
        {
            if (!File.Exists(FilePath))
                return new List<Child>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Child>>(json) ?? new List<Child>();
        }

        /**
         * \brief Speichert die übergebene Liste von Kindern in der JSON-Datei.
         * 
         * Die Daten werden formatiert (eingerückt) als JSON gespeichert.
         * 
         * \param children Liste der Child-Objekte, die gespeichert werden sollen.
         */
        public void SaveChildren(List<Child> children)
        {
            var json = JsonSerializer.Serialize(children, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
