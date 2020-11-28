namespace CoachEasy.Services.Data.Client
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Web.ViewModels;

    public class ClientService : IClientService
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;

        public ClientService(IDeletableEntityRepository<Client> repository)
        {
            this.clientRepository = repository;
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
    }
}
