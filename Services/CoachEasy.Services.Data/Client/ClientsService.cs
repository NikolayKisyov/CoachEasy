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
    using CoachEasy.Services.Data.Workout;
    using CoachEasy.Web.ViewModels;

    public class ClientsService : IClientsService
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;
        private readonly IRepository<WorkoutClients> workoutClientsrepository;

        public ClientsService(
            IDeletableEntityRepository<Client> repository,
            IRepository<WorkoutClients> workoutClientsrepository)
        {
            this.clientRepository = repository;
            this.workoutClientsrepository = workoutClientsrepository;
        }

        public async Task<bool> AddWorkoutToClientList(string id, string userId)
        {
            var client = this.clientRepository.All()
                                 .Where(x => x.UserId == userId)
                                 .Select(x => new {x.Id,x.WorkoutsList })
                                 .First();

            if (!client.WorkoutsList.Any(x => x.ClientId == client.Id && x.WorkoutId == id))
            {
                var workout = new WorkoutClients { WorkoutId = id, ClientId = client.Id };

                await this.workoutClientsrepository.AddAsync(workout);
                await this.workoutClientsrepository.SaveChangesAsync();
                return true;
            }

            return false;
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

        public Client GetClientById(string userId)
        {
            var client = this.clientRepository.AllAsNoTracking().FirstOrDefault(x => x.UserId == userId);

            return client;
        }
    }
}
