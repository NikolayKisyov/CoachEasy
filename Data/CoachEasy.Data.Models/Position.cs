namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Data.Common.Models;
    using CoachEasy.Data.Models.Enums;

    public class Position : BaseDeletableModel<string>
    {
        public Position()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Skills = new HashSet<Skill>();
            this.Players = new HashSet<Player>();
        }

        [Required]
        public PositionName Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
