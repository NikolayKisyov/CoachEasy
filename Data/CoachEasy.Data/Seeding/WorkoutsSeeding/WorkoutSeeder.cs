namespace CoachEasy.Data.Seeding.WorkoutsSeeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Models;
    using Newtonsoft.Json;

    public class WorkoutSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var positions = dbContext.Positions.Select(x => x).ToList();

            if (dbContext.Workouts.Any())
            {
                return;
            }

            StreamReader reader = new StreamReader(@"wwwroot\Workouts.json");

            string json = await reader.ReadToEndAsync();

            var workoutsJson = JsonConvert.DeserializeObject<List<PositionWorkoutJsonDTO>>(json);

            var workouts = new List<Workout>();

            foreach (var position in positions)
            {
                foreach (var entry in workoutsJson.Where(x => x.Position == position.Name.ToString()))
                {
                    foreach (var workout in entry.Workouts)
                    {
                        var workoutToAdd = new Workout
                        {
                            Name = workout.WorkoutName,
                            Description = workout.Description,
                            ImageUrl = workout.ImageUrl,
                            VideoUrl = workout.VideoUrl,
                            PositionId = position.Id,
                            Position = position,
                        };

                        workouts.Add(workoutToAdd);
                    }
                }
            }

            await dbContext.Workouts.AddRangeAsync(workouts);
            await dbContext.SaveChangesAsync();
        }
    }
}
