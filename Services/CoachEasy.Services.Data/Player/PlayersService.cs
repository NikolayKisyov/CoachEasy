namespace CoachEasy.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.Controllers;
    using Microsoft.EntityFrameworkCore;

    public class PlayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> repository;

        public PlayersService(IDeletableEntityRepository<Player> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllPlayersAsync<T>()
        {
            var players = await this.repository
                .AllAsNoTracking()
                .To<T>()
                .ToListAsync();

            return players;
        }
    }
}
