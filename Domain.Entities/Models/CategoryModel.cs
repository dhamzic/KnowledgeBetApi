using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
    }
}
