namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class WorkoutsList : BaseDeletableModel<string>
    {
        public WorkoutsList()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string WorkoutId { get; set; }

        public virtual Workout Workout { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
