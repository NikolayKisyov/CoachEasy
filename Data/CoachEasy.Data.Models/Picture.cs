namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class Picture : BaseDeletableModel<string>
    {
        public Picture()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Players = new HashSet<Player>();
            this.Coaches = new HashSet<Coach>();
            this.Skills = new HashSet<Skill>();
        }

        [Required]
        public string Url { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Coach> Coaches { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }
    }
}
