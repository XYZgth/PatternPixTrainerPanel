using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Repositories
{
    public interface IChildRepository
    {
        void SaveChildren(List<Child> children);

        void SaveTrainings(List<Training> trainings);

        List<Child> LoadChildren();
    }

}