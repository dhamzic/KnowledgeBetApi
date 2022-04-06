using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IGameRepository
    {
        Task<List<PlayedGamesModel>> GetAllPlayedGames();
        Task<GameModel> CreateNewGame(DateTime date, List<int> players, List<int> questions, int winner);
        Task<GameModel> GetGame(int id);
    }
}
