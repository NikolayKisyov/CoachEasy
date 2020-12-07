namespace CoachEasy.Web.ViewModels.Courses
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CoursesListViewModel : PagingViewModel
    {
        public IEnumerable<CourseInListViewModel> Courses { get; set; }
    }
}
