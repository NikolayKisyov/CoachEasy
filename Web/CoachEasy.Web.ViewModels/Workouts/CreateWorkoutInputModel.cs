namespace CoachEasy.Web.ViewModels.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Common;
    using CoachEasy.Data.Models.Enums;
    using Microsoft.AspNetCore.Http;

    public class CreateWorkoutInputModel
    {
        [Required]
        [Display(Name = "Workout name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = GlobalConstants.DescriptionErrorMessage)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "For position")]
        public PositionName PositionName { get; set; }

        [Required]
        [Display(Name = "Embed video link")]
        public string VideoUrl { get; set; }

        [Required]
        [Display(Name = "Workout picture")]
        public IFormFile Image { get; set; }
    }
}
