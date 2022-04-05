using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class QuestionModel
    {
        public string Text { get; set; }
        public virtual List<SubcategoryModel> Subcategories { get; set; }
        public virtual List<QuestionOptionModel> Options { get; set; }
    }
}
