namespace CoachEasy.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Web.Controllers;
    using Microsoft.EntityFrameworkCore;

    public class PlayersService : IPlayersService
    {
        private readonly IDeletableEntityRepository<Player> repository;

        public PlayersService(IDeletableEntityRepository<Player> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<PlayerViewModel>> GetAllPlayersAsync()
        {
            var players = await this.repository
                .AllAsNoTracking()
                .Select(x => new PlayerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PositionName = x.Position.Name,
                    TeamName = x.TeamName,
                    Championships = x.Championships,
                    Height = x.Height,
                    Weight = x.Weight,
                    Experience = x.Experience,
                    ImageUrl = x.ImageUrl,
                }).ToListAsync();

            return players;
        }
    }
}
