namespace CoachEasy.Web.ViewModels.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Models;
    using CoachEasy.Data.Models.Enums;
    using CoachEasy.Services.Mapping;

    public class WorkoutInListViewModel : IMapFrom<Workout>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public PositionName PositionName { get; set; }

        public string VideoUrl { get; set; }

        public string PictureUrl { get; set; }

        public string ImageUrl { get; set; }

        public Coach AddedByCoach { get; set; }
    }
}
