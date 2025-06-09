using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatternPixTrainerPanel.Data;
using PatternPixTrainerPanel.Model;

namespace PatternPixTrainerPanel.Repositories
{
    internal class DBChildRepository : IChildRepository
    {
        private readonly PatternPixDbContext _context;

        public DBChildRepository(PatternPixDbContext context)
        {
            _context = context;
        }

        public void SaveChildren(List<Child> children)
        {
            _context.Children.AddRange(children);
            _context.SaveChanges();
        }

        public void SaveTrainings(List<Training> trainings)
        {
            _context.Trainings.AddRange(trainings);
            _context.SaveChanges();
        }
        public List<Child> LoadChildren()
        {
            return _context.Children
                .Include(c => c.Trainings)
                .ToList();
        }

    }
}
