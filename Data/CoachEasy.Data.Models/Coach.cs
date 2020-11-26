namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class Coach : BaseDeletableModel<string>
    {
        public Coach()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Clients = new HashSet<CoachClients>();
            this.CoachWorkouts = new HashSet<Workout>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int Experience { get; set; }

        public string Description { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Picture))]
        public string PictureId { get; set; }

        [Required]
        public virtual Picture Picture { get; set; }

        public virtual ICollection<CoachClients> Clients { get; set; }

        public virtual ICollection<Workout> CoachWorkouts { get; set; }
        // TODO: Implement Courses in the future
    }
}
