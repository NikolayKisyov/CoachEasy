namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Web.ViewModels;
    using Moq;
    using Xunit;

    public class PositionsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Position>> repository;

        public PositionsServiceTests()
        {
            this.repository = new Mock<IDeletableEntityRepository<Position>>();
        }

        [Fact]
        public async Task GetPlayerAsyncShouldReturnCorrectPlayer()
        {
            var positions = new List<Position>
            {
                new Position
                {
                     Name = PositionName.PointGuard,
                     Description = "Point guard position",
                     Playstyle = "You play like a point guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "peshosId123",
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
                           Name = "Gosho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 4,
                           Description = "Another short description for this one",
                           Experience = "5 years",
                           Height = "1.8m",
                           Weight = "85kg",
                          },
                     },
                },
                new Position
                {
                     Name = PositionName.ShootingGuard,
                     Description = "Shooting guard position",
                     Playstyle = "You play like a shooting guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "toshosId123",
                           Name = "Toshoo Player",
                           TeamName = "SoftUni Coders",
                           Championships = 2,
                           Description = "Some short descr for this one",
                           Experience = "5 years",
                           Height = "1.90m",
                           Weight = "91kg",
                          },
                     },
                },
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(() => positions.AsQueryable());
            var service = new PositionsService(this.repository.Object);

            var position = await service.GetPlayerAsync("peshosId123");

            Assert.NotNull(service);
            Assert.Equal("PointGuard", position.PositionName.ToString());
        }

        [Fact]
        public async Task GetPlayerAsyncShouldReturnNullWithIdNotFound()
        {
            var positions = new List<Position>
            {
                new Position
                {
                     Name = PositionName.PointGuard,
                     Description = "Point guard position",
                     Playstyle = "You play like a point guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "peshosId123",
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
                           Name = "Gosho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 4,
                           Description = "Another short description for this one",
                           Experience = "5 years",
                           Height = "1.8m",
                           Weight = "85kg",
                          },
                     },
                },
                new Position
                {
                     Name = PositionName.ShootingGuard,
                     Description = "Shooting guard position",
                     Playstyle = "You play like a shooting guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "toshosId123",
                           Name = "Toshoo Player",
                           TeamName = "SoftUni Coders",
                           Championships = 2,
                           Description = "Some short descr for this one",
                           Experience = "5 years",
                           Height = "1.90m",
                           Weight = "91kg",
                          },
                     },
                },
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(() => positions.AsQueryable());
            var service = new PositionsService(this.repository.Object);

            var position = await service.GetPlayerAsync("peshoNotFound123");

            Assert.Null(position);
        }

        [Fact]
        public void GetPositionByIdShouldReturnTheRightPosition()
        {
            var positions = new List<Position>
            {
                new Position
                {
                     Id = "position1",
                     Name = PositionName.PointGuard,
                     Description = "Point guard position",
                     Playstyle = "You play like a point guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "peshosId123",
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
                           Name = "Gosho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 4,
                           Description = "Another short description for this one",
                           Experience = "5 years",
                           Height = "1.8m",
                           Weight = "85kg",
                          },
                     },
                },
                new Position
                {
                     Id = "position2",
                     Name = PositionName.ShootingGuard,
                     Description = "Shooting guard position",
                     Playstyle = "You play like a shooting guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "toshosId123",
                           Name = "Toshoo Player",
                           TeamName = "SoftUni Coders",
                           Championships = 2,
                           Description = "Some short descr for this one",
                           Experience = "5 years",
                           Height = "1.90m",
                           Weight = "91kg",
                          },
                     },
                },
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(() => positions.AsQueryable());
            var service = new PositionsService(this.repository.Object);

            var position = service.GetPositionById("position1");

            Assert.Equal("PointGuard", position.Name.ToString());
            Assert.Equal(2, position.Players.Count());
        }

        [Fact]
        public void GetPositionByNameShouldReturnTheRightPosition()
        {
            var positions = new List<Position>
            {
                new Position
                {
                     Id = "position1",
                     Name = PositionName.PointGuard,
                     Description = "Point guard position",
                     Playstyle = "You play like a point guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "peshosId123",
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
                           Name = "Gosho Player",
                           TeamName = "SoftUni Coders",
                           Championships = 4,
                           Description = "Another short description for this one",
                           Experience = "5 years",
                           Height = "1.8m",
                           Weight = "85kg",
                          },
                     },
                },
                new Position
                {
                     Id = "position2",
                     Name = PositionName.ShootingGuard,
                     Description = "Shooting guard position",
                     Playstyle = "You play like a shooting guard",
                     Players = new List<Player>
                     {
                          new Player
                          {
                           Id = "toshosId123",
                           Name = "Toshoo Player",
                           TeamName = "SoftUni Coders",
                           Championships = 2,
                           Description = "Some short descr for this one",
                           Experience = "5 years",
                           Height = "1.90m",
                           Weight = "91kg",
                          },
                     },
                },
            };

            this.repository.Setup(r => r.AllAsNoTracking()).Returns(() => positions.AsQueryable());
            var service = new PositionsService(this.repository.Object);

            var name = PositionName.PointGuard;
            var position = service.GetPositionByName(name);

            Assert.Equal("PointGuard", position.Name.ToString());
            Assert.NotNull(position);
            this.repository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }
    }
}
