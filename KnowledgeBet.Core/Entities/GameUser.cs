using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class GameUser
    {
        public int UserId { get; set; }
        public int GameId { get; set; }

        public virtual User User { get; set; }
        public virtual Game Game { get; set; }

        public bool HasWon { get; set; }
    }
}
