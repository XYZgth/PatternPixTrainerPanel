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
    public class FileChildRepository
    {
        private const string FilePath = "..\\..\\..\\..\\children.json";

        public List<Child> LoadChildren()
        {
            if (!File.Exists(FilePath))
                return new List<Child>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Child>>(json) ?? new List<Child>();
        }

        public void SaveChildren(List<Child> children)
        {
            var json = JsonSerializer.Serialize(children, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
