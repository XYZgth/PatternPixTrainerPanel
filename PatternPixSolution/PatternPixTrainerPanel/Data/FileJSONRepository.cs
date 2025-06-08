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
    public class FileJSONRepository
    {
        private readonly string _filePath;

        public FileJSONRepository(string type)
        {
            // Dynamisch den Pfad festlegen
            if (type == "children")
            {
                _filePath = "..\\..\\..\\..\\children.json";
            }
            else
            {
                _filePath = "..\\..\\..\\..\\trainings.json";
            }
        }

        /**
         * \brief Lädt die Liste der Kinder aus der JSON-Datei.
         * 
         * Prüft zunächst, ob die Datei existiert. Falls nicht, wird eine leere Liste zurückgegeben.
         * 
         * \return Liste der geladenen Child-Objekte.
         */
        // Generische Load-Methode für beliebige Typen
        public List<T> Load<T>()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        // Generische Save-Methode für beliebige Typen
        public void Save<T>(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
