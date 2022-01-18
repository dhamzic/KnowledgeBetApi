using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Models
{
    public class NewQuestionDTO
    {
        public string Text { get; set; }
        public int SubcategoryId { get; set; }
        public List<QuestionOptionDTO> Options { get; set; }
    }
}
