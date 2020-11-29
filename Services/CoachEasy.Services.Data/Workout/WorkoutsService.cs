namespace CoachEasy.Services.Data.Workout
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;
    using CoachEasy.Services.Data.Cloudinary;
    using CoachEasy.Services.Mapping;
    using CoachEasy.Web.ViewModels.Workouts;

    public class WorkoutsService : IWorkoutsService
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Workout> workoutsRepository;
        private readonly IDeletableEntityRepository<Position> positionsRepository;
        private readonly IDeletableEntityRepository<Coach> coachesRepository;

        public WorkoutsService(
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Workout> workoutsRepository,
            IDeletableEntityRepository<Position> positionsRepository,
            IDeletableEntityRepository<Coach> coachesRepository)
        {
            this.cloudinaryService = cloudinaryService;
            this.workoutsRepository = workoutsRepository;
            this.positionsRepository = positionsRepository;
            this.coachesRepository = coachesRepository;
        }

        public async Task CreateWorkoutAsync(CreateWorkoutInputModel input, string userId)
        {
            string folderName = "workout_images";
            var inputPicture = input.Image;
            var pictureUrl = await this.cloudinaryService.UploadPhotoAsync(inputPicture, inputPicture.FileName, folderName);

            var position = this.positionsRepository.AllAsNoTracking().First(x => x.Name == input.PositionName);
            var coach = this.coachesRepository.AllAsNoTracking().FirstOrDefault(x => x.UserId == userId);

            var workout = new Workout
            {
                Name = input.Name,
                Description = input.Description,
                VideoUrl = input.VideoUrl,
                PositionId = position.Id,
                Position = position,
                Picture = new Picture { Url = pictureUrl },
                CoachId = coach.Id,
                AddedByCoach = coach,
            };

            await this.workoutsRepository.AddAsync(workout);
            await this.workoutsRepository.SaveChangesAsync();
        }

        //public IEnumerable<T> GetAll<T>()
        //{
        //    return this.workoutsRepository
        //        .AllAsNoTracking()
        //        .OrderByDescending(x => x.CreatedOn)
        //        .To<T>()
        //        .ToList();
        //}
    }
}
