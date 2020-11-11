namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class Skill : BaseDeletableModel<string>
    {
        public Skill()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Position))]
        public string PositionId { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Picture))]
        public string PictureId { get; set; }

        [Required]
        public virtual Picture Picture { get; set; }
    }
}
