namespace CoachEasy.Web.ViewModels.Courses
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Services.Mapping;

    public class CourseInListViewModel : IMapFrom<Course>
    {
        public string Id { get; set; }

        public bool HasApplied { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public PositionName PositionName { get; set; }

        public string Description { get; set; }

        public Coach Coach { get; set; }

        public string PictureUrl { get; set; }

    }
}
