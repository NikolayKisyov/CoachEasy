namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Data.Repositories;
    using CoachEasy.Services.Data.Client;
    using CoachEasy.Services.Data.Votes;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class VotesServiceTests
    {
        [Fact]
        public async Task GetAverageVotesShouldReturnCorrectValue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfRepository<Vote>(dbContext);
            var clientRepository = new EfDeletableEntityRepository<Client>(dbContext);
            var workoutslistRepository = new EfDeletableEntityRepository<WorkoutsList>(dbContext);

            var clientsService = new ClientsService(clientRepository, workoutslistRepository);
            var service = new VotesService(repository, clientsService);

            var vote1 = new Vote() { Id = 1, CoachId = "id", Value = 4, };
            var vote2 = new Vote() { Id = 2, CoachId = "id", Value = 2, };
            var vote3 = new Vote() { Id = 3, CoachId = "id", Value = 5, };
            var vote4 = new Vote() { Id = 4, CoachId = "id2", Value = 4, };

            dbContext.Add(vote1);
            dbContext.Add(vote2);
            dbContext.Add(vote3);
            dbContext.Add(vote4);
            await dbContext.SaveChangesAsync();

            var result = service.GetAverageVotes("id");
            var resultTwo = service.GetAverageVotes("id2");

            Assert.Equal(3.6666666666666665, result);
            Assert.Equal(4, resultTwo);
        }

        [Fact]
        public async Task SetVoteShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                  .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfRepository<Vote>(dbContext);
            var clientRepository = new EfDeletableEntityRepository<Client>(dbContext);

            var client = new Client
            {
                Id = "clientId",
                Name = "random",
                Phone = "088",
                PositionPlayed = PositionName.Center,
                User = new ApplicationUser { Id = "clientuserId" , Email = "clientemail@abv.bg" },
                UserId = "clientuserId",
            };

            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            var workoutslistRepository = new EfDeletableEntityRepository<WorkoutsList>(dbContext);

            var clientsService = new ClientsService(clientRepository, workoutslistRepository);
            var service = new VotesService(repository, clientsService);

            await service.SetVoteAsync("coachId", "clientuserId", 2);

            var count = repository.All();
            var vote = await repository.All().FirstAsync(x => x.CoachId == "coachId");
            Assert.Equal(1, await count.CountAsync());
            Assert.Equal(2, vote.Value);
        }

        [Fact]
        public async Task SetVoteShouldChangeVoteValueCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfRepository<Vote>(dbContext);
            var clientRepository = new EfDeletableEntityRepository<Client>(dbContext);

            var client = new Client
            {
                Id = "clientId",
                Name = "random",
                Phone = "088",
                PositionPlayed = PositionName.Center,
                User = new ApplicationUser { Id = "clientuserId", Email = "clientemail@abv.bg" },
                UserId = "clientuserId",
            };

            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            var workoutslistRepository = new EfDeletableEntityRepository<WorkoutsList>(dbContext);

            var clientsService = new ClientsService(clientRepository, workoutslistRepository);
            var service = new VotesService(repository, clientsService);

            await service.SetVoteAsync("coachId", "clientuserId", 2);

            await service.SetVoteAsync("coachId", "clientuserId", 5);

            var count = repository.All();
            var vote = await repository.All().FirstAsync(x => x.CoachId == "coachId");

            Assert.Equal(1, await count.CountAsync());
            Assert.Equal(5, vote.Value);
        }

        [Fact]
        public async Task SetVoteShouldApplyCorrectValues()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfRepository<Vote>(dbContext);
            var clientRepository = new EfDeletableEntityRepository<Client>(dbContext);
            var client = new Client
            {
                Id = "clientId",
                Name = "random",
                Phone = "088",
                PositionPlayed = PositionName.Center,
                User = new ApplicationUser { Id = "clientuserId", Email = "clientemail@abv.bg" },
                UserId = "clientuserId",
            };

            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            var workoutslistRepository = new EfDeletableEntityRepository<WorkoutsList>(dbContext);

            var clientsService = new ClientsService(clientRepository, workoutslistRepository);
            var service = new VotesService(repository, clientsService);

            await service.SetVoteAsync("coachId", "clientuserId", 4);

            var vote = await repository.All().FirstAsync(x => x.CoachId == "coachId");

            Assert.Equal("coachId", vote.CoachId);
            Assert.Equal("clientId", vote.ClientId);
            Assert.Equal(4, vote.Value);
        }
    }
}
