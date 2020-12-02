namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class WorkoutClients
    {
        public WorkoutClients()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string WorkoutId { get; set; }

        public virtual Workout Workout { get; set; }

        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
