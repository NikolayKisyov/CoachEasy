﻿namespace CoachEasy.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Models;

    public class PositionViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Playstyle { get; set; }

        public string PlayerImageUrl { get; set; }

        public ICollection<Workout> Workouts { get; set; }
    }
}
