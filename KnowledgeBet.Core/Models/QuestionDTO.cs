using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Models
{
    public class QuestionDTO
    {
        public string Text { get; set; }
        public virtual List<SubcategoryDTO> Subcategories { get; set; }
        public virtual List<QuestionOptionDTO> Options { get; set; }
    }
}
