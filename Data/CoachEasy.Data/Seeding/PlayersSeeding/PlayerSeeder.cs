namespace CoachEasy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Data.Seeding.SeedingData;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    public class PlayerSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Players.Any())
            {
                return;
            }

            StreamReader reader = new StreamReader(@"wwwroot\Players.json");

            string json = await reader.ReadToEndAsync();

            var playersJson = JsonConvert.DeserializeObject<List<PlayerJsonDTO>>(json);

            var playersList = new List<Player>();

            foreach (var p in playersJson)
            {
                var playerPosition = new Position { Name = (PositionName)Enum.Parse(typeof(PositionName), p.Position) };

                var player = new Player
                {
                    Name = p.Name,
                    TeamName = p.Team,
                    Position = playerPosition,
                    Description = p.Description,
                    PositionId = playerPosition.Id,
                    Championships = p.Championships,
                    Height = p.Height,
                    Weight = p.Weight,
                    Experience = p.Experience,
                    ImageUrl = p.ImageUrl,
                };
                playerPosition.Players.Add(player);
                playersList.Add(player);
            }

            await dbContext.Players.AddRangeAsync(playersList);
            await dbContext.SaveChangesAsync();
        }

    }
}
