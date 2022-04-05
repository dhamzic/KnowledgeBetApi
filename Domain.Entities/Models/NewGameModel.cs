using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class NewGameModel
    {
        public DateTime Date { get; set; }
        public List<int> PlayersId { get; set; }
        public List<int> QuestionsId { get; set; }
        public int PlayerWinnerId { get; set; }
    }
}
