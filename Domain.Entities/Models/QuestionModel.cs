using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class QuestionModel
    {
        public int Id { get; set;}
        public string Text { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
        public List<QuestionOptionModel> Options { get; set; }
    }
}
