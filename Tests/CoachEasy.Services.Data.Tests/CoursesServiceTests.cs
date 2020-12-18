namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Services.Data.Client;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Data.Course;
    using CoachEasy.Web.ViewModels.Coaches;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using Xunit;

    public class CoursesServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Course>> courseRepository;
        private readonly Mock<IDeletableEntityRepository<CourseClients>> courseClientsRepository;
        private readonly Mock<IDeletableEntityRepository<Coach>> coachRepository;
        private readonly Mock<IRepository<WorkoutsList>> workoutslistRepository;
        private readonly Mock<IDeletableEntityRepository<Client>> clientRepository;
        private readonly Mock<ICloudinaryService> moqCloudinaryService;
        private readonly CoachesService coachesService;
        private readonly ClientsService clientsService;
        private readonly CoursesService coursesService;

        public CoursesServiceTests()
        {
            this.courseRepository = new Mock<IDeletableEntityRepository<Course>>();
            this.courseClientsRepository = new Mock<IDeletableEntityRepository<CourseClients>>();
            this.coachRepository = new Mock<IDeletableEntityRepository<Coach>>();
            this.workoutslistRepository = new Mock<IRepository<WorkoutsList>>();
            this.clientRepository = new Mock<IDeletableEntityRepository<Client>>();
            this.moqCloudinaryService = new Mock<ICloudinaryService>();
            this.coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);
            this.clientsService = new ClientsService(this.clientRepository.Object, this.workoutslistRepository.Object);
            this.coursesService = new CoursesService(this.coachesService, this.clientsService, this.courseRepository.Object, this.courseClientsRepository.Object, this.moqCloudinaryService.Object);
        }

        [Fact]
        public void GetCountShouldReturnCorrectValue()
        {
            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Coach>
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
            }.AsQueryable());

            this.courseRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Course>
            {
                new Course
                {
                      Name = "test",
                      PositionName = PositionName.SmallForward,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description",
                      Picture = new Picture { Url = "testurl" },
                      Coach = new Coach
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
                      CoachId = "c1",
                },
            }.AsQueryable());

            var count = this.coursesService.GetCount();
            Assert.Equal(1, count);
            this.courseRepository.Verify(r => r.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task GetAllShouldReturnCollectionOfCourses()
        {
            this.courseRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Course>
            {
                new Course
                {
                      Name = "test",
                      PositionName = PositionName.SmallForward,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description",
                      Picture = new Picture { Url = "testurl" },
                      Coach = new Coach
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
                      CoachId = "c1",
                },
                new Course
                {
                      Name = "test2",
                      PositionName = PositionName.Center,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description2",
                      Picture = new Picture { Url = "testurl2" },
                      Coach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "3213123312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                      CoachId = "c2",
                },
            }.AsQueryable());

            this.clientRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Client>
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
            }.AsQueryable());

            var collection = await this.coursesService.GetAll("clientUser", 1, 12);
            Assert.Equal(2, collection.Count());
            this.courseRepository.Verify(r => r.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task CreateCourseShouldCreateCourseSuccessfullyAndAddItToCoach()
        {
            var courses = new List<Course>
            {
                new Course
                {
                      Name = "test",
                      PositionName = PositionName.SmallForward,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description",
                      Picture = new Picture { Url = "testurl" },
                      Coach = new Coach
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
                      CoachId = "c1",
                },
                new Course
                {
                      Name = "test2",
                      PositionName = PositionName.Center,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description2",
                      Picture = new Picture { Url = "testurl2" },
                      Coach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "3213123312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                      CoachId = "c2",
                },
            };

            this.courseRepository.Setup(r => r.AllAsNoTracking()).Returns(() => courses.AsQueryable());

            this.coachRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Coach>
            {
               new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "3213123312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
            }.AsQueryable());

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

            this.courseRepository.Setup(r => r.AddAsync(It.IsAny<Course>())).Callback((Course course) => courses.Add(course));

            var model = new CreateCourseInputModel
            {
                Name = "testcourse",
                Description = "test description for course",
                PositionName = PositionName.ShootingGuard,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Image = file,
            };

            await this.coursesService.CreateCourseAsync(model, "coachuser2");
            Assert.Contains(courses, x => x.Name == "testcourse");
            Assert.Equal(3, courses.Count);
            this.courseRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public async Task AddClientToCourseShouldCreateAndAddApplicationAndSkipItIfItAlreadyExists()
        {
            var courses = new List<Course>
            {
                new Course
                {
                      Id = "course1",
                      Name = "test",
                      PositionName = PositionName.SmallForward,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description",
                      Picture = new Picture { Url = "testurl" },
                      Coach = new Coach
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
                      CoachId = "c1",
                },
                new Course
                {
                      Id = "course2",
                      Name = "test2",
                      PositionName = PositionName.Center,
                      StarDate = DateTime.Now,
                      EndDate = DateTime.Now,
                      Description = "Test description2",
                      Picture = new Picture { Url = "testurl2" },
                      Coach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 5,
                               Phone = "3213123312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2", Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                      CoachId = "c2",
                },
            };

            this.courseRepository.Setup(r => r.AllAsNoTracking()).Returns(() => courses.AsQueryable());

            this.clientRepository.Setup(r => r.All()).Returns(() => new List<Client>
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
            }.AsQueryable());

            var courseClients = new List<CourseClients>();

            this.courseClientsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => courseClients.AsQueryable());

            this.courseClientsRepository.Setup(r => r.AddAsync(It.IsAny<CourseClients>())).Callback((CourseClients application) => courseClients.Add(application));

            var result = await this.coursesService.AddClientToCourse("course1", "clientUser");
            var result2 = await this.coursesService.AddClientToCourse("course1", "clientUser");
            Assert.Equal("created", result);
            Assert.Equal("contained", result2);
            this.courseRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }
    }
}
