namespace CoachEasy.Web.ViewModels.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Models;
    using CoachEasy.Services.Mapping;

    public class WorkoutViewModel : IMapFrom<Workout>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PositionName { get; set; }

        public string VideoUrl { get; set; }

        public string PictureUrl { get; set; }

        public Coach AddedByCoach { get; set; }
    }
}
