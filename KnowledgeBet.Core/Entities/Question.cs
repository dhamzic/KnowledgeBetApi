using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class Question
    {
        public Question()
        {
            this.Subcategories = new HashSet<Subcategory>();
            this.Options = new HashSet<QuestionOption>();
            this.Games = new HashSet<Game>();
        }
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
        public virtual ICollection<QuestionOption> Options { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
