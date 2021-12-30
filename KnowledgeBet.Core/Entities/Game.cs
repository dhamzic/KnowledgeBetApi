using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Subcategory SubcategoryId { get; set; }
        public ICollection<Question> Questions { get; set; }
        //public ICollection<User> Players { get; set; }
        public User Winner { get; set; }
    }
}
