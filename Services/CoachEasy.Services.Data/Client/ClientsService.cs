namespace CoachEasy.Services.Data.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Models;
    using CoachEasy.Services.Data.Workout;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels;
    using CoachEasy.Web.ViewModels.Workouts;
    using Microsoft.EntityFrameworkCore;

    public class ClientsService : IClientsService
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;
        private readonly IRepository<WorkoutsList> workoutClientsrepository;
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IPositionsService positionsService;

        public ClientsService(
            IDeletableEntityRepository<Client> repository,
            IRepository<WorkoutsList> workoutClientsrepository)
        {
            this.clientRepository = repository;
            this.workoutClientsrepository = workoutClientsrepository;
        }

        public async Task<string> AddWorkoutToClientList(string id, string userId)
        {
            var client = this.GetClient(userId);
            if (id == null)
            {
                return "invalid";
            }

            if (!client.WorkoutsList.Any(x => x.ClientId == client.Id && x.WorkoutId == id))
            {
                var workout = new WorkoutsList { WorkoutId = id, ClientId = client.Id };

                await this.workoutClientsrepository.AddAsync(workout);
                await this.workoutClientsrepository.SaveChangesAsync();
                return "added";
            }

            return "contained";
        }

        public async Task<bool> CreateClientAsync(CreateClientInputModel input, ApplicationUser user)
        {
            var client = new Client
            {
                Name = input.FullName,
                HasBasketballExperience = input.HasExperience,
                Phone = input.Phone,
                PositionPlayed = input.PositionPlayed,
                User = user,
                UserId = user.Id,
            };

            if (client != null)
            {
                await this.clientRepository.AddAsync(client);
                await this.clientRepository.SaveChangesAsync();

                return true;
            }

            throw new InvalidOperationException(GlobalConstants.InvalidOperationExceptionWhileCreatingClient);
        }

        public async Task<IEnumerable<WorkoutInListViewModel>> GetWorkouts(string userId, int page, int itemsPerPage)
        {
            var result = await this.workoutClientsrepository
                .AllAsNoTracking()
                .Where(x => x.Client.UserId == userId)
                .Select(x => new WorkoutInListViewModel
                {
                    Id = x.Workout.Id,
                    Name = x.Workout.Name,
                    PositionName = x.Workout.Position.Name,
                    Description = x.Workout.Description,
                    ImageUrl = x.Workout.ImageUrl,
                    PictureUrl = x.Workout.Picture.Url,
                    VideoUrl = x.Workout.VideoUrl,
                    AddedByCoach = x.Workout.AddedByCoach,
                })
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .ToListAsync();

            return result;
        }

        //GetClient uses ClientDto so we can get the WorkoutList of the client.
        public ClientDto GetClient(string userId)
        {
            return this.clientRepository.All()
                                 .Where(x => x.UserId == userId)
                                 .Select(x => new ClientDto
                                 {
                                     Id = x.Id,
                                     WorkoutsList = x.WorkoutsList,
                                 })
                                 .First();
        }

        public int GetCount(string userId)
        {
            var client = this.GetClient(userId);

            return client.WorkoutsList.Count;
        }

        public async Task Delete(string workoutId, string userId)
        {
            var client = this.clientRepository.AllAsNoTracking().FirstOrDefault(x => x.UserId == userId);

            var workout = this.workoutClientsrepository.All()
                .FirstOrDefault(x => x.WorkoutId == workoutId && x.ClientId == client.Id);

            this.workoutClientsrepository.Delete(workout);
            await this.workoutClientsrepository.SaveChangesAsync();
        }
    }
}
