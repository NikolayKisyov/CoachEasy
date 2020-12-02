namespace CoachEasy.Web.ViewModels.Players
{
    using CoachEasy.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TestVM
    {
        public string Name { get; set; }

        public ICollection<WorkoutClients> Workouts { get; set; }
    }
}
