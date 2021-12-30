using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Subcategory> Subcategories { get; set; }
        public ICollection<Game> Games { get; set; }

    }
}
