using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Models
{
    public class NewGameDTO
    {
        public DateTime Date { get; set; }
        public List<int> PlayersId { get; set; }
        public List<int> QuestionsId { get; set; }
        public int PlayerWinnerId { get; set; }
    }
}
