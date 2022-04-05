using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IKnowledgeBetService
    {
        Task<List<UserModel>> GetAllPlayers();
        Task<List<GameModel>> GetAllPlayedGames();
        Task<bool> CreateNewQuestion(NewQuestionModel newQuestion);
        Task<bool> CreateNewGame(NewGameModel newGame);
        Task<bool> DeleteQuestion(int questionId);
        Task<bool> DeactivateQuestion(int questionId);



    }
}
