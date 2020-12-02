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
    using CoachEasy.Web.ViewModels;

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

        public Coach GetCoachByUserId(string id)
        {
            var coach = this.coachRepository.AllAsNoTracking().FirstOrDefault(x => x.UserId == id);

            return coach;
        }
    }
}
