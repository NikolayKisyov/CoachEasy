namespace CoachEasy.Services.Data.Picture
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Common;
    using CoachEasy.Data.Common.Repositories;
    using CoachEasy.Data.Models;

    public class PictureService : IPictureService
    {
        private readonly IDeletableEntityRepository<Picture> pictures;

        public PictureService(IDeletableEntityRepository<Picture> pictures)
        {
            this.pictures = pictures;
        }

        public async Task<string> AddPictureAsync(string url)
        {
            var picture = new Picture() { Url = url };

            await this.pictures.AddAsync(picture);
            var res = await this.pictures.SaveChangesAsync();
            if (res < 0)
            {
                throw new InvalidOperationException(GlobalConstants.InvalidOperationExceptionInPictureService);
            }
            else
            {
                return picture.Id;
            }
        }
    }
}
