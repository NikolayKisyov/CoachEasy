namespace CoachEasy.Data.Seeding.PositionsSeeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class PositionSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var positions = dbContext.Positions.ToList();

            if (!string.IsNullOrEmpty(positions[0].Description))
            {
                return;
            }

            StreamReader reader = new StreamReader(@"wwwroot\Positions.json");

            string json = await reader.ReadToEndAsync();

            var positionsJson = JsonConvert.DeserializeObject<List<PositionJsonDTO>>(json);

            foreach (var item in positionsJson)
            {
                foreach (var position in positions)
                {
                    if (position.Name.ToString() == item.PositionName)
                    {
                        position.Playstyle = item.Playstyle;
                        position.Description = item.Description;
                        break;
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
