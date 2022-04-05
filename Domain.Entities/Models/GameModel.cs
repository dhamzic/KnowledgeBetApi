using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Models
{
    public class GameModel
    {
        public List<UserModel> Players { get; set; }
        public UserModel Winner { get; set; }
        public DateTime Date { get; set; }
    }
}
