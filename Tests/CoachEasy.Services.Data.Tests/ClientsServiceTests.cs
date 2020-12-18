namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Data;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Data.Repositories;
    using CoachEasy.Services.Data.Client;
    using CoachEasy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ClientsServiceTests
    {
        private readonly Mock<IRepository<WorkoutsList>> workoutslistRepository;
        private readonly Mock<IDeletableEntityRepository<Client>> clientRepository;
        private readonly ClientsService clientsService;

        public ClientsServiceTests()
        {
            this.workoutslistRepository = new Mock<IRepository<WorkoutsList>>();
            this.clientRepository = new Mock<IDeletableEntityRepository<Client>>();
            this.clientsService = new ClientsService(this.clientRepository.Object, this.workoutslistRepository.Object);
        }

        [Fact]
        public async Task CreateClientShouldWorkWithCorrectData()
        {
            var clients = new List<Client>();
            this.clientRepository.Setup(r => r.AllAsNoTracking()).Returns(() => clients.AsQueryable());

            this.clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Callback((Client client) => clients.Add(client));

            var model = new CreateClientInputModel
            {
                FullName = "Client One",
                PositionPlayed = PositionName.PowerForward,
                Email = "client@abv.bg",
                HasExperience = true,
                Phone = "088",
            };

            var appUser = new ApplicationUser { Id = "clientuserId", Email = "clientemail@abv.bg" };

            await this.clientsService.CreateClientAsync(model, appUser);

            Assert.Contains(clients, x => x.Name == "Client One");
            Assert.Single(clients);
            this.clientRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task CreateClientShouldThrowIOEWithInvalidData()
        {
            var clients = new List<Client>();
            this.clientRepository.Setup(r => r.AllAsNoTracking()).Returns(() => clients.AsQueryable());

            this.clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Callback((Client client) => clients.Add(client));

            var model = new CreateClientInputModel
            {
                FullName = string.Empty,
                PositionPlayed = PositionName.PowerForward,
                Email = string.Empty,
                HasExperience = false,
                Phone = string.Empty,
            };

            var appUser = new ApplicationUser { Id = string.Empty, Email = string.Empty };

            await Assert.ThrowsAsync<InvalidOperationException>(() => this.clientsService.CreateClientAsync(model, appUser));
        }

        [Fact]
        public async Task AddWorkoutToClientListShouldReturnAddedWithCorrectData()
        {
            var workoutsList = new List<WorkoutsList>();
            this.workoutslistRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workoutsList.AsQueryable());

            this.workoutslistRepository.Setup(r => r.AddAsync(It.IsAny<WorkoutsList>())).Callback((WorkoutsList workout) => workoutsList.Add(workout));

            var clients = new List<Client>
            {
                 new Client
                {
                    Id = "client1",
                    Name = "clientov",
                    Phone = "088",
                    HasBasketballExperience = true,
                    PositionPlayed = PositionName.PowerForward,
                    User = new ApplicationUser { Id = "clientUser"},
                    UserId = "clientUser",
                },
            };

            this.clientRepository.Setup(r => r.All()).Returns(() => clients.AsQueryable());

            this.clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Callback((Client client) => clients.Add(client));

            var result = await this.clientsService.AddWorkoutToClientList("w1", "clientUser");
            Assert.Equal("added", result);
            Assert.Single(workoutsList);
            this.workoutslistRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task AddWorkoutToClientListShouldContainedIfOneAlreadyExists()
        {
            var workoutsList = new List<WorkoutsList>
            {
                new WorkoutsList
                {
                    ClientId = "client1",
                    WorkoutId = "w1",
                },
            };

            this.workoutslistRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workoutsList.AsQueryable());

            this.workoutslistRepository.Setup(r => r.AddAsync(It.IsAny<WorkoutsList>())).Callback((WorkoutsList workout) => workoutsList.Add(workout));

            var clients = new List<Client>
            {
                 new Client
                {
                    Id = "client1",
                    Name = "clientov",
                    Phone = "088",
                    HasBasketballExperience = true,
                    PositionPlayed = PositionName.PowerForward,
                    User = new ApplicationUser { Id = "clientUser"},
                    UserId = "clientUser",
                    WorkoutsList = new List<WorkoutsList>
                                  {
                                   new WorkoutsList
                                   {
                                       ClientId = "client1",
                                       WorkoutId = "w1",
                                   },
                                  },
                },
            };

            this.clientRepository.Setup(r => r.All()).Returns(() => clients.AsQueryable());

            this.clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Callback((Client client) => clients.Add(client));

            var result = await this.clientsService.AddWorkoutToClientList("w1", "clientUser");
            Assert.Equal("contained", result);
            Assert.Single(workoutsList);
            this.workoutslistRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task AddWorkoutToClientListShouldReturnInvalidIfWorkoutIdIsNull()
        {
            var workoutsList = new List<WorkoutsList>();
            this.workoutslistRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workoutsList.AsQueryable());

            this.workoutslistRepository.Setup(r => r.AddAsync(It.IsAny<WorkoutsList>())).Callback((WorkoutsList workout) => workoutsList.Add(workout));

            var clients = new List<Client>
            {
                 new Client
                {
                    Id = "client1",
                    Name = "clientov",
                    Phone = "088",
                    HasBasketballExperience = true,
                    PositionPlayed = PositionName.PowerForward,
                    User = new ApplicationUser { Id = "clientUser"},
                    UserId = "clientUser",
                },
            };

            this.clientRepository.Setup(r => r.All()).Returns(() => clients.AsQueryable());

            this.clientRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Callback((Client client) => clients.Add(client));

            var result = await this.clientsService.AddWorkoutToClientList(null, "clientUser");
            Assert.Equal("invalid", result);
            Assert.Empty(workoutsList);
            this.workoutslistRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task GetWorkoutsShouldReturnAllWorkoutsAddedInClientsList()
        {
            var client = new Client
            {
                Id = "client1",
                Name = "clientov",
                Phone = "088",
                HasBasketballExperience = true,
                PositionPlayed = PositionName.PowerForward,
                User = new ApplicationUser { Id = "clientUser" },
                UserId = "clientUser",
                WorkoutsList = new List<WorkoutsList>
                                  {
                                   new WorkoutsList
                                   {
                                       ClientId = "client1",
                                       WorkoutId = "w1",
                                   },
                                  },
            };

            var workoutsList = new List<WorkoutsList>
            {
                  new WorkoutsList
                                   {
                                       ClientId = "client1",
                                       Client = client,
                                       WorkoutId = "w1",
                                       Workout = new Workout
                                       {
                                           Id = "w1",
                                           Name = "Workout One",
                                           Description = "Some kind of workout description",
                                           Position = new Position
                                           {
                                                Id = "PositionOne",
                                                Name = PositionName.ShootingGuard,
                                                Description = "Shooting guard position",
                                                Playstyle = "You play like a shooting guard",
                                           },
                                           VideoUrl = "test youtube link",
                                           PositionId = "PositionOne",
                                           Picture = new Picture { Id = "pic", Url = "test url" },
                                           PictureId = "pic",
                                           ImageUrl = "testimg",
                                           AddedByCoach = new Coach
                                           {
                                               Id = "c1",
                                               Name = "Coach1",
                                               Description = "desc1",
                                               Experience = 2,
                                               Phone = "321312312",
                                               Email = "coach@abv.bg",
                                               User = new ApplicationUser { Id = "coachuser" },
                                               UserId = "coachuser",
                                               Picture = new Picture { Id = "cpic", Url = "test xurl" },
                                               PictureId = "cpic",
                                           },
                                       },
                                   },
            };
            this.workoutslistRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workoutsList.AsQueryable());

            var clients = new List<Client> { client };

            this.clientRepository.Setup(r => r.All()).Returns(() => clients.AsQueryable());

            var result = await this.clientsService.GetWorkouts("clientUser", 1, 12);
            Assert.Single(result);
            Assert.Equal("test youtube link", result.ToList().First().VideoUrl);
            Assert.Equal(PositionName.ShootingGuard, result.ToList().First().PositionName);
            this.workoutslistRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCountShouldReturnCorrectClientWorkoutsCount()
        {
            var client = new Client
            {
                Id = "client1",
                Name = "clientov",
                Phone = "088",
                HasBasketballExperience = true,
                PositionPlayed = PositionName.PowerForward,
                User = new ApplicationUser { Id = "clientUser" },
                UserId = "clientUser",
                WorkoutsList = new List<WorkoutsList>
                                  {
                                   new WorkoutsList
                                   {
                                       ClientId = "client1",
                                       WorkoutId = "w1",
                                   },
                                  },
            };

            var clients = new List<Client> { client };

            this.clientRepository.Setup(r => r.All()).Returns(() => clients.AsQueryable());

            var result = this.clientsService.GetCount("clientUser");
            Assert.Equal(1, result);
            this.clientRepository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task DeleteShouldWorkCorrectlyWithExistingWorkoutInClientsList()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var client = new Client
            {
                Id = "client1",
                Name = "clientov",
                Phone = "088",
                HasBasketballExperience = true,
                PositionPlayed = PositionName.PowerForward,
                User = new ApplicationUser { Id = "clientUser" },
                UserId = "clientUser",
            };

            var workoutList = new WorkoutsList
            {
                ClientId = "client1",
                Client = client,
                WorkoutId = "w1",
                Workout = new Workout
                {
                    Id = "w1",
                    Name = "Workout One",
                    Description = "Some kind of workout description",
                    Position = new Position
                    {
                        Id = "PositionOne",
                        Name = PositionName.ShootingGuard,
                        Description = "Shooting guard position",
                        Playstyle = "You play like a shooting guard",
                    },
                    VideoUrl = "test youtube link",
                    PositionId = "PositionOne",
                    Picture = new Picture { Id = "pic", Url = "test url" },
                    PictureId = "pic",
                    ImageUrl = "testimg",
                },
            };

            var clientsRepo = new EfDeletableEntityRepository<Client>(dbContext);
            var wlistRepo = new EfDeletableEntityRepository<WorkoutsList>(dbContext);

            var clientsDbService = new ClientsService(clientsRepo, wlistRepo);

            await dbContext.Clients.AddAsync(client);
            await dbContext.WorkoutsList.AddAsync(workoutList);
            await dbContext.SaveChangesAsync();

            await clientsDbService.Delete("w1", "clientUser");
            var result = wlistRepo.All().Where(x => x.ClientId == "client1")
                                 .FirstOrDefault();
            Assert.Null(result);
        }
    }
}
