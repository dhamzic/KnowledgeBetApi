using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Entities
{
    public class Game
    {
        public Game()
        {
            this.Players = new HashSet<GameUser>();
            this.Questions = new HashSet<Question>();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<GameUser> Players { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
