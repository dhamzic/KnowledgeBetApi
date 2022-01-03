using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class GameWon
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public int GamePlayedId { get; set; }
        public virtual Game GamePlayed { get; set; }
    }
}
