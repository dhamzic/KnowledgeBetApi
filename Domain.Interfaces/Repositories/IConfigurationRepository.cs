using Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IConfigurationRepository
    {
        Task<List<HomeComponentTileModel>> GetHomeComponentTiles();
    }
}
