using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class NewQuestionModel
    {
        public string Text { get; set; }
        public int SubcategoryId { get; set; }
        public List<QuestionOptionModel> Options { get; set; }
    }
}
