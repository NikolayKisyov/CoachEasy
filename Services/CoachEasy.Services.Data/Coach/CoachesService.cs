namespace CoachEasy.Services.Data.Coach
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Data.Picture;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels;
    using CoachEasy.Web.ViewModels.Coaches;
    using Microsoft.EntityFrameworkCore;

    public class CoachesService : ICoachesService
    {
        private readonly IDeletableEntityRepository<Coach> coachRepository;
        private readonly ICloudinaryService cloudinaryService;

        public CoachesService(
            IDeletableEntityRepository<Coach> coachRepository,
            ICloudinaryService cloudinaryService)
        {
            this.coachRepository = coachRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<bool> CreateCoachAsync(CreateCoachInputModel input, ApplicationUser user)
        {
            string folderName = "coach_images";
            var inputPicture = input.UserImage;
            var pictureUrl = await this.cloudinaryService.UploadPhotoAsync(inputPicture, inputPicture.FileName, folderName);

            var coach = new Coach
            {
                Name = input.FullName,
                Email = input.Email,
                Description = input.Description,
                Phone = input.Phone,
                Experience = input.Experience,
                Picture = new Picture { Url = pictureUrl },
                User = user,
                UserId = user.Id,
            };

            if (coach != null && coach.Experience > 0)
            {
                await this.coachRepository.AddAsync(coach);
                await this.coachRepository.SaveChangesAsync();

                return true;
            }

            throw new InvalidOperationException(GlobalConstants.InvalidOperationExceptionWhileCreatingCoach);
        }

        public async Task<IEnumerable<T>> GetAllCoachesAsync<T>()
        {
            var coaches = await this.coachRepository
                .AllAsNoTracking()
                .To<T>()
                .ToListAsync();

            return coaches;
        }

        public async Task<CoachViewModel> GetCoachById(string id)
        {
            var coach = await this.coachRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new CoachViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Experience = x.Experience,
                    Phone = x.Phone,
                    Email = x.Email,
                    PictureUrl = x.Picture.Url,
                    CoachWorkoutsCount = x.CoachWorkouts.Count(),
                    CoursesCount = x.Courses.Count(),
                    Description = x.Description,
                    AverageVote = !x.Votes.Any() ? 0 : x.Votes.Average(x => x.Value),
                }).FirstOrDefaultAsync();

            return coach;
        }

        public Coach GetCoachByUserId(string id)
        {
            var coach = this.coachRepository.AllAsNoTracking().FirstOrDefault(x => x.UserId == id);

            return coach;
        }
    }
}
