namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels;
    using CoachEasy.Web.ViewModels.Coaches;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class CoachesServiceTests
    {
        private readonly Mock<ICloudinaryService> moqCloudinaryService;
        private readonly Mock<IDeletableEntityRepository<Coach>> coachRepository;
        private readonly CoachesService coachesService;

        public CoachesServiceTests()
        {
            this.moqCloudinaryService = new Mock<ICloudinaryService>();
            this.coachRepository = new Mock<IDeletableEntityRepository<Coach>>();
            this.coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);
            AutoMapperConfig.RegisterMappings(Assembly.Load("CoachEasy.Web.ViewModels"));
        }

        [Fact]
        public async Task CreateCoachShouldWorkWithCorrectData()
        {
            var coaches = new List<Coach>();
            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => coaches.AsQueryable());

            this.coachRepository.Setup(r => r.AddAsync(It.IsAny<Coach>())).Callback((Coach coach) => coaches.Add(coach));

            // Arrange
            var fileMock = new Mock<IFormFile>();

            // Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var file = fileMock.Object;

            var model = new CreateCoachInputModel
            {
                FullName = "Coach One",
                Email = "test@abv.bg",
                Experience = 5,
                Description = "test description for coach",
                Phone = "088",
                UserImage = file,
            };

            var appUser = new ApplicationUser { Id = "coachuserId", Email = "coachemail@abv.bg" };

            var result = await this.coachesService.CreateCoachAsync(model, appUser);

            Assert.Contains(coaches, x => x.Name == "Coach One");
            Assert.Single(coaches);
            Assert.True(result);
            this.coachRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task CreateCoachShouldThrowIOEWithInvalidData()
        {
            var coaches = new List<Coach>();
            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => coaches.AsQueryable());

            this.coachRepository.Setup(r => r.AddAsync(It.IsAny<Coach>())).Callback((Coach coach) => coaches.Add(coach));

            // Arrange
            var fileMock = new Mock<IFormFile>();

            // Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var file = fileMock.Object;

            var model = new CreateCoachInputModel
            {
                FullName = string.Empty,
                Email = string.Empty,
                Experience = 0,
                Description = string.Empty,
                Phone = string.Empty,
                UserImage = file,
            };

            var appUser = new ApplicationUser { Id = string.Empty, Email = string.Empty };

            await Assert.ThrowsAsync<InvalidOperationException>(() => this.coachesService.CreateCoachAsync(model, appUser));
        }

        [Fact]
        public async Task GetAllShouldReturnCollectionOfCoaches()
        {
            var coaches = new List<Coach>
            {
                new Coach
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
                new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "32133213212312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
            };

            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => coaches.AsQueryable());

            var models = await this.coachesService.GetAllCoachesAsync<CoachViewModel>();

            Assert.Equal(2, models.Count());
            this.coachRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetCoachByUserIdShouldWorkWhenCoachExists()
        {
            var coaches = new List<Coach>
            {
                new Coach
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
                new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "32133213212312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
            };

            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => coaches.AsQueryable());

            var coach = this.coachesService.GetCoachByUserId("coachuser2");

            Assert.Equal("c2", coach.Id);
            Assert.Equal(5, coach.Experience);
            this.coachRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetCoachByIdShouldReturnCoachViewModel()
        {
            var coaches = new List<Coach>
            {
                new Coach
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
                new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "32133213212312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
            };

            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => coaches.AsQueryable());

            var coach = await this.coachesService.GetCoachById("c1");

            Assert.Equal("c1", coach.Id);
            Assert.Equal(2, coach.Experience);
            Assert.Equal(0, coach.CoachWorkoutsCount);
            this.coachRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }
    }
}
