using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class Category
    {
        public Category()
        {
            this.Subcategories = new HashSet<Subcategory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
