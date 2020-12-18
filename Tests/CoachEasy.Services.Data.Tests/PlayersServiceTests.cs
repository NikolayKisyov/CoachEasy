namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels.Players;
    using Moq;
    using Xunit;

    public class PlayersServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Player>> repository;

        public PlayersServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Player>>();
            AutoMapperConfig.RegisterMappings(Assembly.Load("CoachEasy.Web.ViewModels"));
        }

        [Fact]
        public void GetAllPlayersShouldReturnCollectionOfPlayers()
        {
            var players = new List<Player>
            {
                    new Player
                          {
                           Id = "peshosId123",
                           Position = new Position
                           {
                                Name = PositionName.ShootingGuard,
                                Description = "Shooting guard position",
                                Playstyle = "You play like a shooting guard",
                           },
                           Name = "Pesho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 6,
                           Description = "Some short description for this one",
                           Experience = "8 years",
                           Height = "2.0m",
                           Weight = "102kg",
                          },
                    new Player
                          {
                           Id = "goshosId123",
                           Position = new Position
                           {
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           Name = "Gosho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 4,
                           Description = "Another short description for this one",
                           Experience = "5 years",
                           Height = "1.8m",
                           Weight = "85kg",
                          },
                    new Player
                          {
                           Id = "toshosId123",
                           Position = new Position
                           {
                                Name = PositionName.Center,
                                Description = "Center guard position",
                                Playstyle = "You play like a center guard",
                           },
                           Name = "Toshoo Player",
                           TeamName = "SoftUni Coders",
                           Championships = 2,
                           Description = "Some short descr for this one",
                           Experience = "5 years",
                           Height = "1.90m",
                           Weight = "91kg",
                          },
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(() => players.AsQueryable());
            var service = new PlayersService(this.repository.Object);

            var playersCollection = service.GetAllPlayersAsync<PlayerViewModel>();

            Assert.Equal(3, playersCollection.Result.Count());
        }
    }
}
