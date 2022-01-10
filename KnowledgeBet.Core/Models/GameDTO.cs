using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Models
{
    public class GameDTO
    {
        public List<UserDTO> Players { get; set; }
        public UserDTO Winner { get; set; }
        public DateTime Date { get; set; }
    }
}
