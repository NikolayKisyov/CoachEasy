namespace CoachEasy.Services.Data.Course
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Client;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Web.ViewModels.Coaches;
    using CoachEasy.Web.ViewModels.Courses;
    using Microsoft.EntityFrameworkCore;

    public class CoursesService : ICoursesService
    {
        private readonly ICoachesService coachesService;
        private readonly IClientsService clientsService;
        private readonly IDeletableEntityRepository<Course> coursesRepository;
        private readonly IDeletableEntityRepository<CourseClients> courseClientsRepository;
        private readonly ICloudinaryService cloudinaryService;

        public CoursesService(
            ICoachesService coachesService,
            IClientsService clientsService,
            IDeletableEntityRepository<Course> coursesRepository,
            IDeletableEntityRepository<CourseClients> courseClientsRepository,
            ICloudinaryService cloudinaryService)
        {
            this.coachesService = coachesService;
            this.clientsService = clientsService;
            this.coursesRepository = coursesRepository;
            this.courseClientsRepository = courseClientsRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<string> AddClientToCourse(string id, string userId)
        {
            var client = this.clientsService.GetClientById(userId);

            if (!this.courseClientsRepository.AllAsNoTracking().Any(x => x.ClientId == client.Id && x.CourseId == id))
            {
                var courseApplication = new CourseClients
                {
                    ClientId = client.Id,
                    CourseId = id,
                };

                await this.courseClientsRepository.AddAsync(courseApplication);
                await this.courseClientsRepository.SaveChangesAsync();

                return "created";
            }

            return "contained";
        }

        public async Task CreateCourseAsync(CreateCourseInputModel input, string userId)
        {
            string folderName = "course_images";
            var inputPicture = input.Image;
            var pictureUrl = await this.cloudinaryService.UploadPhotoAsync(inputPicture, inputPicture.FileName, folderName);

            var coach = this.coachesService.GetCoachByUserId(userId);

            var course = new Course
            {
                Name = input.Name,
                PositionName = input.PositionName,
                StarDate = input.StartDate,
                EndDate = input.EndDate,
                Description = input.Description,
                Picture = new Picture { Url = pictureUrl },
                CoachId = coach.Id,
            };

            coach.Courses.Add(course);

            await this.coursesRepository.AddAsync(course);
            await this.coursesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CourseInListViewModel>> GetAll(string userId, int page, int itemsPerPage)
        {
            var client = this.clientsService.GetClientById(userId);

            var courses = this.coursesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(x => new CourseInListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    HasApplied = client == null ? false : this.courseClientsRepository.All().Any(c => c.ClientId == client.Id && c.CourseId == x.Id),
                    PositionName = x.PositionName,
                    StartDate = x.StarDate,
                    EndDate = x.EndDate,
                    Description = x.Description,
                    Coach = x.Coach,
                    PictureUrl = x.Picture.Url,
                })
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .ToList();

            return courses;
        }

        public int GetCount()
        {
            return this.coursesRepository.AllAsNoTracking().Count();
        }
    }
}
