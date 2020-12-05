namespace CoachEasy.Web.ViewModels.Coaches
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Models;
    using CoachEasy.Services.Mapping;

    public class CoachViewModel : IMapFrom<Coach>
    {
        public string Name { get; set; }

        public int Experience { get; set; }

        public string Description { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PictureUrl { get; set; }

        public int ClientsCount { get; set; }

        public int CoachWorkoutsCount { get; set; }
    }
}
