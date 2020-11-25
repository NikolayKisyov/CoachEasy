namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Data.Common.Models;
    using CoachEasy.Data.Models.Enums;

    public class Client : BaseDeletableModel<string>
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Coaches = new HashSet<CoachClients>();
            this.WorkoutsList = new HashSet<Workout>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool HasBasketballExperience { get; set; }

        [Required]
        public PositionName PositionPlayed { get; set; }

        [Required]
        public string Phone { get; set; }

        public virtual ICollection<CoachClients> Coaches { get; set; }

        public virtual ICollection<Workout> WorkoutsList { get; set; }
    }
}
