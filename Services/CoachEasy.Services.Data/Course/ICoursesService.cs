namespace CoachEasy.Services.Data.Course
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using CoachEasy.Web.ViewModels.Coaches;
    using CoachEasy.Web.ViewModels.Courses;

    public interface ICoursesService
    {
        Task<string> AddClientToCourse(string id, string userId);

        Task CreateCourseAsync(CreateCourseInputModel input, string userId);

        int GetCount();

        Task<IEnumerable<CourseInListViewModel>> GetAll(string userId, int page, int itemsPerPage);
    }
}
