namespace CoachEasy.Services.Data.Course
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Coach;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels.Coaches;

    public class CoursesService : ICoursesService
    {
        private readonly ICoachesService coachesService;
        private readonly IDeletableEntityRepository<Course> coursesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public CoursesService(
            ICoachesService coachesService,
            IDeletableEntityRepository<Course> coursesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.coachesService = coachesService;
            this.coursesRepository = coursesRepository;
            this.cloudinaryService = cloudinaryService;
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

        public IEnumerable<T> GetAll<T>(int page, int itemsPerPage)
        {
            var dealerships = this.coursesRepository.AllAsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<T>().ToList();
            return dealerships;
        }

        public int GetCount()
        {
            return this.coursesRepository.AllAsNoTracking().Count();
        }
    }
}
