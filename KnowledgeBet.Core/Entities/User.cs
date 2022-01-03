using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Entities
{
    public class User
    {
        public User()
        {
            this.GamesPlayed = new HashSet<Game>();
            this.GamesWon = new HashSet<GameWon>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public virtual ICollection<Game> GamesPlayed { get; set; }
        public virtual ICollection<GameWon> GamesWon { get; set; }

    }
}
