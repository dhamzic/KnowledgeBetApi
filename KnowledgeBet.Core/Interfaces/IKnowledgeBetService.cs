using KnowledgeBet.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBet.Core.Interfaces
{
    public interface IKnowledgeBetService
    {
        List<UserDTO> GetAllPlayers();
    }
}
