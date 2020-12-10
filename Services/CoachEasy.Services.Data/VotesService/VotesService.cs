namespace CoachEasy.Services.Data.VotesService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Client;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IClientsService clientsService;

        public VotesService(
            IRepository<Vote> votesRepository,
            IClientsService clientsService)
        {
            this.votesRepository = votesRepository;
            this.clientsService = clientsService;
        }

        public double GetAverageVotes(string coachId)
        {
            return this.votesRepository.All()
                .Where(x => x.CoachId == coachId)
                .Average(x => x.Value);
        }

        public async Task SetVoteAsync(string coachId, string userId, int value)
        {
            var client = this.clientsService.GetClientById(userId);
            var vote = this.votesRepository.All().FirstOrDefault(x => x.CoachId == coachId && x.ClientId == client.Id);

            if (vote == null)
            {
                vote = new Vote
                {
                    CoachId = coachId,
                    ClientId = client.Id,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = (byte)value;

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
