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
     * \brief Verwaltet das Laden und Speichern von Daten im JSON-Format.
     * 
     * Diese Klasse unterstützt generisches Laden und Speichern von Datenobjekten
     * aus bzw. in eine JSON-Datei, abhängig vom angegebenen Typ.
     */
    public class FileJSONRepository
    {
        private readonly string _filePath;

        /**
         * \brief Initialisiert eine neue Instanz der FileJSONRepository-Klasse.
         * 
         * Je nach angegebenem Typ (z. B. "children" oder "trainings") wird der Pfad zur JSON-Datei gesetzt.
         * 
         * \param type Der Datentyp, z. B. "children" oder "trainings", zur Auswahl des Dateipfads.
         */
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
         * \brief Lädt eine Liste von Objekten aus der zugewiesenen JSON-Datei.
         * 
         * Wenn die Datei nicht existiert, wird eine leere Liste zurückgegeben.
         * Die Methode ist generisch und unterstützt alle serialisierbaren Typen.
         * 
         * \tparam T Der Typ der Objekte, die geladen werden sollen.
         * \return Eine Liste von deserialisierten Objekten des Typs T.
         */
        public List<T> Load<T>()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        /**
         * \brief Speichert eine Liste von Objekten in der zugewiesenen JSON-Datei.
         * 
         * Die Methode serialisiert die übergebenen Objekte im JSON-Format
         * und schreibt die Ausgabe in die Zieldatei. Die Ausgabe ist eingerückt formatiert.
         * 
         * \tparam T Der Typ der Objekte, die gespeichert werden sollen.
         * \param items Die Liste von Objekten, die gespeichert werden sollen.
         */
        public void Save<T>(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
