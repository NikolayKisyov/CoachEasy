namespace CoachEasy.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class PositionsService : IPositionsService
    {
        private readonly IDeletableEntityRepository<Position> repository;

        public PositionsService(IDeletableEntityRepository<Position> repository)
        {
            this.repository = repository;
        }

        public async Task<PositionViewModel> GetPlayerAsync(string id)
        {
            var player = await this.repository
                .AllAsNoTracking()
                .Where(x => x.Players.First().Id == id).Select(x => new PositionViewModel
                {
                    Id = x.Id,
                    Name = x.Name.ToString(),
                    Description = x.Description,
                    Playstyle = x.Playstyle,
                    PlayerImageUrl = x.Players.First().ImageUrl,
                    Workouts = x.Workouts,
                }).FirstOrDefaultAsync();

            return player;
        }

        public Position GetPositionById(string id)
        {
           var position = this.repository.AllAsNoTracking().First(x => x.Id == id);

           return position;
        }

        public Position GetPositionByName(PositionName name)
        {
            var position = this.repository.AllAsNoTracking().First(x => x.Name == name);

            return position;
        }
    }
}
