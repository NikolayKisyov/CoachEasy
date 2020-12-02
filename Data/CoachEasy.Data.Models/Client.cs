namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using CoachEasy.Data.Common.Models;
    using CoachEasy.Data.Models.Enums;

    public class Client : BaseDeletableModel<string>
    {
        public Client()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Coaches = new HashSet<CoachClients>();
            this.WorkoutsList = new HashSet<WorkoutClients>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool HasBasketballExperience { get; set; }

        [Required]
        public PositionName PositionPlayed { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CoachClients> Coaches { get; set; }

        public virtual ICollection<WorkoutClients> WorkoutsList { get; set; }
    }
}
