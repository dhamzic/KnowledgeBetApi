using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class GameUser
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public bool HasWon { get; set; }
    }
}
