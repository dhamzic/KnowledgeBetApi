using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Entities
{
    public class Subcategory
    {
        public Subcategory()
        {
            this.Questions = new HashSet<Question>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

    }
}