using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<UserModel>> GetAllPlayers();
        Task<UserModel> CreateNewPlayer(string firstName, string lastName);
        Task<UserModel> GetPlayer(int id);
    }
}