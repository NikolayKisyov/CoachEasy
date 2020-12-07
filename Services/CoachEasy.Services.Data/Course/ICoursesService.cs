namespace CoachEasy.Services.Data.Course
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Web.ViewModels.Coaches;

    public interface ICoursesService
    {
        Task CreateCourseAsync(CreateCourseInputModel input, string userId);

        int GetCount();

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage);
    }
}
