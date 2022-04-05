using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Entities
{
    public class User
    {
        public User()
        {
            this.GamesPlayed = new HashSet<GameUser>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public virtual ICollection<GameUser> GamesPlayed { get; set; }

    }
}
