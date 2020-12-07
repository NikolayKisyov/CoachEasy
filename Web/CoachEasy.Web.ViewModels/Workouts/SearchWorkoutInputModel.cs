namespace CoachEasy.Web.ViewModels.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SearchWorkoutInputModel
    {
        [Display(Name = "Point Guard")]
        public bool PointGuard { get; set; }

        [Display(Name = "Shooting Guard")]
        public bool ShootingGuard { get; set; }

        [Display(Name = "Small Forward")]
        public bool SmallForward { get; set; }

        [Display(Name = "Power Forward")]
        public bool PowerForward { get; set; }

        [Display(Name = "Center")]
        public bool Center { get; set; }
    }
}
