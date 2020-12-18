namespace CoachEasy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using CoachEasy.Data;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Data.Repositories;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Data.Workout;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels.Workouts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class WorkoutsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Workout>> workoutsRepository;
        private readonly Mock<IDeletableEntityRepository<Position>> positionsRepository;
        private readonly Mock<ICloudinaryService> moqCloudinaryService;
        private readonly Mock<IDeletableEntityRepository<Coach>> coachRepository;

        public WorkoutsServiceTests()
        {
            this.workoutsRepository = new Mock<IDeletableEntityRepository<Workout>>();
            this.positionsRepository = new Mock<IDeletableEntityRepository<Position>>();
            this.coachRepository = new Mock<IDeletableEntityRepository<Coach>>();
            this.moqCloudinaryService = new Mock<ICloudinaryService>();
            AutoMapperConfig.RegisterMappings(Assembly.Load("CoachEasy.Web.ViewModels"));
        }

        [Fact]
        public async Task AddWorkoutShouldCreateWorkoutAndAddItToCoach()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
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
            };

            this.positionsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Position>
            {
                new Position
                {
                     Id = "PositionOne",
                     Name = PositionName.ShootingGuard,
                     Description = "Shooting guard position",
                     Playstyle = "You play like a shooting guard",
                },
            }.AsQueryable());
            var positionsService = new PositionsService(this.positionsRepository.Object);

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

            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

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

            this.workoutsRepository.Setup(r => r.AddAsync(It.IsAny<Workout>())).Callback((Workout workout) => workouts.Add(workout));

            var inputModel = new CreateWorkoutInputModel
            {
                Name = "Workout for beginners",
                Description = "This workout is for beginners and it will help them",
                PositionName = PositionName.ShootingGuard,
                VideoUrl = "testvideourl",
                Image = file,
            };

            await service.CreateAsync(inputModel, "coachuser");
            Assert.Contains(workouts, x => x.Name == "Workout for beginners");
            Assert.Equal(2, workouts.Count);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Never);
        }

        [Fact]
        public void GetCountShouldWorkCorrectly()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
                           Name = "Workout One",
                           Description = "Some kind of workout description",
                           Position = new Position
                           {
                                Name = PositionName.ShootingGuard,
                                Description = "Shooting guard position",
                                Playstyle = "You play like a shooting guard",
                           },
                           VideoUrl = "test youtube link",
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           VideoUrl = "test youtube link 2",
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);

            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var count = service.GetCount();
            Assert.Equal(2, count);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetWorkoutByIdShouldReturnTheCorrectWorkout()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
                           Name = "Workout One",
                           Description = "Some kind of workout description",
                           Position = new Position
                           {
                                Name = PositionName.ShootingGuard,
                                Description = "Shooting guard position",
                                Playstyle = "You play like a shooting guard",
                           },
                           VideoUrl = "test youtube link",
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           VideoUrl = "test youtube link 2",
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);

            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var workout = service.GetWorkoutById("workoutId123");
            Assert.Equal("Workout One", workout.Name);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetWorkoutByIdShouldReturnNullIfWorkoutDoesntExist()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
                           Name = "Workout One",
                           Description = "Some kind of workout description",
                           Position = new Position
                           {
                                Name = PositionName.ShootingGuard,
                                Description = "Shooting guard position",
                                Playstyle = "You play like a shooting guard",
                           },
                           VideoUrl = "test youtube link",
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           VideoUrl = "test youtube link 2",
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);

            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var workout = service.GetWorkoutById("notexistingworkout");
            Assert.Null(workout);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllShouldReturnAllWorkoutsIfNotFiltered()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
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
                               Picture = new Picture { Id = "cpic" ,Url = "test xurl" },
                               PictureId = "cpic",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Id = "PositionTwo",
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           PositionId = "PositionTwo",
                           VideoUrl = "test youtube link 2",
                           Picture = new Picture { Id = "2pic", Url = "test 2url" },
                           PictureId = "2pic",
                           ImageUrl = "2testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 8,
                               Phone = "322312312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2" ,Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId789",
                           Name = "Workout Three",
                           Description = "Some kind of random description again",
                           Position = new Position
                           {
                                Id = "PositionThree",
                                Name = PositionName.Center,
                                Description = "Center position",
                                Playstyle = "You play like a center",
                           },
                           PositionId = "PositionThree",
                           VideoUrl = "test youtube link 3",
                           Picture = new Picture { Id = "3pic", Url = "test 3url" },
                           PictureId = "3pic",
                           ImageUrl = "3testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c3",
                               Name = "Coach3",
                               Description = "desc3",
                               Experience = 5,
                               Phone = "32235412",
                               Email = "coach3@abv.bg",
                               User = new ApplicationUser { Id = "coachuser3" },
                               UserId = "coachuser3",
                               Picture = new Picture { Id = "cpic3", Url = "test xurl3" },
                               PictureId = "cpic3",
                           },
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);
            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var inputModel = new SearchWorkoutInputModel
            {
                PointGuard = false,
                ShootingGuard = false,
                SmallForward = false,
                PowerForward = false,
                Center = false,
            };

            var workoutsAll = service.GetSearchedPositions<WorkoutInListViewModel>(inputModel, 1, 12);
            Assert.Equal(3, workoutsAll.Workouts.Count());
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllShouldNotMakeChangesIfAllPositionsAreFiltered()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
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
                               Picture = new Picture { Id = "cpic" ,Url = "test xurl" },
                               PictureId = "cpic",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Id = "PositionTwo",
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           PositionId = "PositionTwo",
                           VideoUrl = "test youtube link 2",
                           Picture = new Picture { Id = "2pic", Url = "test 2url" },
                           PictureId = "2pic",
                           ImageUrl = "2testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 8,
                               Phone = "322312312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2" ,Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId789",
                           Name = "Workout Three",
                           Description = "Some kind of random description again",
                           Position = new Position
                           {
                                Id = "PositionThree",
                                Name = PositionName.Center,
                                Description = "Center position",
                                Playstyle = "You play like a center",
                           },
                           PositionId = "PositionThree",
                           VideoUrl = "test youtube link 3",
                           Picture = new Picture { Id = "3pic", Url = "test 3url" },
                           PictureId = "3pic",
                           ImageUrl = "3testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c3",
                               Name = "Coach3",
                               Description = "desc3",
                               Experience = 5,
                               Phone = "32235412",
                               Email = "coach3@abv.bg",
                               User = new ApplicationUser { Id = "coachuser3" },
                               UserId = "coachuser3",
                               Picture = new Picture { Id = "cpic3", Url = "test xurl3" },
                               PictureId = "cpic3",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId21312312",
                           Name = "Workout Four",
                           Description = "Some kind of random description agaixn",
                           Position = new Position
                           {
                                Id = "PositionFour",
                                Name = PositionName.SmallForward,
                                Description = "Small Forward position",
                                Playstyle = "You play like a SF",
                           },
                           PositionId = "PositionFour",
                           VideoUrl = "test youtube link 4",
                           Picture = new Picture { Id = "4pic", Url = "test 4url" },
                           PictureId = "4pic",
                           ImageUrl = "4testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c4",
                               Name = "Coach4",
                               Description = "desc4",
                               Experience = 6,
                               Phone = "32235212",
                               Email = "coach4@abv.bg",
                               User = new ApplicationUser { Id = "coachuser4" },
                               UserId = "coachuser4",
                               Picture = new Picture { Id = "cpic4", Url = "test xurl4" },
                               PictureId = "cpic4",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId2312312312312",
                           Name = "Workout Five",
                           Description = "Some kind of randommm description again",
                           Position = new Position
                           {
                                Id = "PositionFive",
                                Name = PositionName.PowerForward,
                                Description = "Power Forward position",
                                Playstyle = "You play like a PF",
                           },
                           PositionId = "PositionFive",
                           VideoUrl = "test youtube link 5",
                           Picture = new Picture { Id = "5pic", Url = "test 5url" },
                           PictureId = "5pic",
                           ImageUrl = "5testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c5",
                               Name = "Coach5",
                               Description = "desc5",
                               Experience = 9,
                               Phone = "34435212",
                               Email = "coach5@abv.bg",
                               User = new ApplicationUser { Id = "coachuser5" },
                               UserId = "coachuser5",
                               Picture = new Picture { Id = "cpic5", Url = "test xurl5" },
                               PictureId = "cpic5",
                           },
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);
            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var inputModel = new SearchWorkoutInputModel
            {
                PointGuard = true,
                ShootingGuard = true,
                SmallForward = true,
                PowerForward = true,
                Center = true,
            };

            var workoutsAll = service.GetSearchedPositions<WorkoutInListViewModel>(inputModel, 1, 12);
            Assert.Empty(workoutsAll.Workouts);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetAllShouldReturnChosenWorkoutWhenFilteredForIt()
        {
            var workouts = new List<Workout>
            {
                    new Workout
                          {
                           Id = "workoutId123",
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
                               Picture = new Picture { Id = "cpic" ,Url = "test xurl" },
                               PictureId = "cpic",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId456",
                           Name = "Workout Two",
                           Description = "Some kind of random description",
                           Position = new Position
                           {
                                Id = "PositionTwo",
                                Name = PositionName.PointGuard,
                                Description = "Point guard position",
                                Playstyle = "You play like a point guard",
                           },
                           PositionId = "PositionTwo",
                           VideoUrl = "test youtube link 2",
                           Picture = new Picture { Id = "2pic", Url = "test 2url" },
                           PictureId = "2pic",
                           ImageUrl = "2testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c2",
                               Name = "Coach2",
                               Description = "desc2",
                               Experience = 8,
                               Phone = "322312312",
                               Email = "coach2@abv.bg",
                               User = new ApplicationUser { Id = "coachuser2" },
                               UserId = "coachuser2",
                               Picture = new Picture { Id = "cpic2" ,Url = "test xurl2" },
                               PictureId = "cpic2",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId789",
                           Name = "Workout Three",
                           Description = "Some kind of random description again",
                           Position = new Position
                           {
                                Id = "PositionThree",
                                Name = PositionName.Center,
                                Description = "Center position",
                                Playstyle = "You play like a center",
                           },
                           PositionId = "PositionThree",
                           VideoUrl = "test youtube link 3",
                           Picture = new Picture { Id = "3pic", Url = "test 3url" },
                           PictureId = "3pic",
                           ImageUrl = "3testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c3",
                               Name = "Coach3",
                               Description = "desc3",
                               Experience = 5,
                               Phone = "32235412",
                               Email = "coach3@abv.bg",
                               User = new ApplicationUser { Id = "coachuser3" },
                               UserId = "coachuser3",
                               Picture = new Picture { Id = "cpic3", Url = "test xurl3" },
                               PictureId = "cpic3",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId21312312",
                           Name = "Workout Four",
                           Description = "Some kind of random description agaixn",
                           Position = new Position
                           {
                                Id = "PositionFour",
                                Name = PositionName.SmallForward,
                                Description = "Small Forward position",
                                Playstyle = "You play like a SF",
                           },
                           PositionId = "PositionFour",
                           VideoUrl = "test youtube link 4",
                           Picture = new Picture { Id = "4pic", Url = "test 4url" },
                           PictureId = "4pic",
                           ImageUrl = "4testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c4",
                               Name = "Coach4",
                               Description = "desc4",
                               Experience = 6,
                               Phone = "32235212",
                               Email = "coach4@abv.bg",
                               User = new ApplicationUser { Id = "coachuser4" },
                               UserId = "coachuser4",
                               Picture = new Picture { Id = "cpic4", Url = "test xurl4" },
                               PictureId = "cpic4",
                           },
                          },
                    new Workout
                          {
                           Id = "workoutId2312312312312",
                           Name = "Workout Five",
                           Description = "Some kind of randommm description again",
                           Position = new Position
                           {
                                Id = "PositionFive",
                                Name = PositionName.PowerForward,
                                Description = "Power Forward position",
                                Playstyle = "You play like a PF",
                           },
                           PositionId = "PositionFive",
                           VideoUrl = "test youtube link 5",
                           Picture = new Picture { Id = "5pic", Url = "test 5url" },
                           PictureId = "5pic",
                           ImageUrl = "5testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c5",
                               Name = "Coach5",
                               Description = "desc5",
                               Experience = 9,
                               Phone = "34435212",
                               Email = "coach5@abv.bg",
                               User = new ApplicationUser { Id = "coachuser5" },
                               UserId = "coachuser5",
                               Picture = new Picture { Id = "cpic5", Url = "test xurl5" },
                               PictureId = "cpic5",
                           },
                          },
            };

            var positionsService = new PositionsService(this.positionsRepository.Object);
            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var inputModel = new SearchWorkoutInputModel
            {
                PointGuard = true,
                ShootingGuard = false,
                SmallForward = false,
                PowerForward = false,
                Center = false,
            };

            var workoutsAll = service.GetSearchedPositions<WorkoutInListViewModel>(inputModel, 1, 12);

            Assert.Equal("Workout Two", workoutsAll.Workouts.First().Name);
            Assert.Single(workoutsAll.Workouts);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetWorkoutForEditShouldReturnCorrectWorkout()
        {
            var workouts = new List<Workout>
            {
                     new Workout
                          {
                           Id = "workoutId2312312312312",
                           Name = "Workout Five",
                           Description = "Some kind of randommm description again",
                           Position = new Position
                           {
                                Id = "PositionFive",
                                Name = PositionName.PowerForward,
                                Description = "Power Forward position",
                                Playstyle = "You play like a PF",
                           },
                           PositionId = "PositionFive",
                           VideoUrl = "test youtube link 5",
                           Picture = new Picture { Id = "5pic", Url = "test 5url" },
                           PictureId = "5pic",
                           ImageUrl = "5testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c5",
                               Name = "Coach5",
                               Description = "desc5",
                               Experience = 9,
                               Phone = "34435212",
                               Email = "coach5@abv.bg",
                               User = new ApplicationUser { Id = "coachuser5" },
                               UserId = "coachuser5",
                               Picture = new Picture { Id = "cpic5", Url = "test xurl5" },
                               PictureId = "cpic5",
                           },
                           CoachId = "c5",
                          },
            };

            this.positionsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Position>
            {
                new Position
                {
                     Id = "PositionFive",
                     Name = PositionName.PowerForward,
                     Description = "Power Forward position",
                     Playstyle = "You play like a PF",
                },
            }.AsQueryable());

            var positionsService = new PositionsService(this.positionsRepository.Object);

            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var workout = service.GetWorkoutForEdit("workoutId2312312312312");
            Assert.Equal("c5", workout.CoachId);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public void GetWorkoutForEditShouldThrowExceptionIfWorkoutNotFound()
        {
            var workouts = new List<Workout>
            {
                     new Workout
                          {
                           Id = "workoutId2312312312312",
                           Name = "Workout Five",
                           Description = "Some kind of randommm description again",
                           Position = new Position
                           {
                                Id = "PositionFive",
                                Name = PositionName.PowerForward,
                                Description = "Power Forward position",
                                Playstyle = "You play like a PF",
                           },
                           PositionId = "PositionFive",
                           VideoUrl = "test youtube link 5",
                           Picture = new Picture { Id = "5pic", Url = "test 5url" },
                           PictureId = "5pic",
                           ImageUrl = "5testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c5",
                               Name = "Coach5",
                               Description = "desc5",
                               Experience = 9,
                               Phone = "34435212",
                               Email = "coach5@abv.bg",
                               User = new ApplicationUser { Id = "coachuser5" },
                               UserId = "coachuser5",
                               Picture = new Picture { Id = "cpic5", Url = "test xurl5" },
                               PictureId = "cpic5",
                           },
                           CoachId = "c5",
                          },
            };

            this.positionsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Position>
            {
                new Position
                {
                     Id = "PositionFive",
                     Name = PositionName.PowerForward,
                     Description = "Power Forward position",
                     Playstyle = "You play like a PF",
                },
            }.AsQueryable());

            var positionsService = new PositionsService(this.positionsRepository.Object);
            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            Assert.Throws<InvalidOperationException>(() => service.GetWorkoutForEdit("notexistingId"));
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.Once);
        }

        [Fact]
        public async Task EditWorkoutShouldUpdateCorrectly()
        {
            var workouts = new List<Workout>
            {
                     new Workout
                          {
                           Id = "workoutId2312312312312",
                           Name = "Workout Five",
                           Description = "Some kind of randommm description again",
                           Position = new Position
                           {
                                Id = "PositionFive",
                                Name = PositionName.PowerForward,
                                Description = "Power Forward position",
                                Playstyle = "You play like a PF",
                           },
                           PositionId = "PositionFive",
                           VideoUrl = "test youtube link 5",
                           Picture = new Picture { Id = "5pic", Url = "test 5url" },
                           PictureId = "5pic",
                           ImageUrl = "5testimg",
                           AddedByCoach = new Coach
                           {
                               Id = "c5",
                               Name = "Coach5",
                               Description = "desc5",
                               Experience = 9,
                               Phone = "34435212",
                               Email = "coach5@abv.bg",
                               User = new ApplicationUser { Id = "coachuser5" },
                               UserId = "coachuser5",
                               Picture = new Picture { Id = "cpic5", Url = "test xurl5" },
                               PictureId = "cpic5",
                           },
                           CoachId = "c5",
                          },
            };

            this.positionsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => new List<Position>
            {
                new Position
                {
                     Id = "PositionFive",
                     Name = PositionName.PowerForward,
                     Description = "Power Forward position",
                     Playstyle = "You play like a PF",
                },
            }.AsQueryable());

            var positionsService = new PositionsService(this.positionsRepository.Object);
            var coachesService = new CoachesService(this.coachRepository.Object, this.moqCloudinaryService.Object);

            this.workoutsRepository.Setup(r => r.AllAsNoTracking()).Returns(() => workouts.AsQueryable());
            var service = new WorkoutsService(this.moqCloudinaryService.Object, this.workoutsRepository.Object, positionsService, coachesService);

            var model = new EditWorkoutViewModel
            {
                Id = "workoutId2312312312312",
                Name = "Changed name",
                Description = "Some kind of randommm description again",
                PositionName = PositionName.PowerForward,
                VideoUrl = "test youtube link CHANGED",
                CoachId = "c5",
            };

            await service.EditAsync(model);
            var workout = service.GetWorkoutById("workoutId2312312312312");
            Assert.Equal("Changed name", workout.Name);
            Assert.Equal("test youtube link CHANGED", workout.VideoUrl);
            this.workoutsRepository.Verify(x => x.AllAsNoTracking(), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteWorkoutShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var position = new Position
            {
                Name = PositionName.PowerForward,
                Description = "Power Forward position",
                Playstyle = "You play like a PF",
            };

            var workout = new Workout
            {
                Name = "Workout Five",
                Description = "Some kind of randommm description again",
                PositionId = position.Id,
                VideoUrl = "test youtube link 5",
                ImageUrl = "5testimg",
            };

            var workoutsRepository = new EfDeletableEntityRepository<Workout>(dbContext);

            var positionsRepository = new EfDeletableEntityRepository<Position>(dbContext);
            var positionsService = new PositionsService(positionsRepository);

            var moqCloudinaryService = new Mock<ICloudinaryService>();

            var coachRepository = new EfDeletableEntityRepository<Coach>(dbContext);
            var coachesService = new CoachesService(coachRepository, moqCloudinaryService.Object);

            var service = new WorkoutsService(moqCloudinaryService.Object, workoutsRepository, positionsService, coachesService);

            var workoutId = workout.Id;

            await dbContext.Positions.AddAsync(position);
            await dbContext.Workouts.AddAsync(workout);
            await dbContext.SaveChangesAsync();

            await service.DeleteAsync(workoutId);
            var workoutxd = service.GetWorkoutById(workoutId);
            Assert.Null(workoutxd);
        }
    }
}
