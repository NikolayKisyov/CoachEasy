namespace CoachEasy.Data.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum PositionName
    {
        [Display(Name = "Point Guard")]
        PointGuard = 0,
        [Display(Name = "Shooting Guard")]
        ShootingGuard = 1,
        [Display(Name = "Power Forward")]
        PowerForward = 2,
        [Display(Name = "Small Forward")]
        SmallForward = 3,
        Center = 4,
    }
}
