namespace CoachEasy.Services.Data.Picture
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPicturesService
    {
        Task<string> AddPictureAsync(string url);
    }
}
