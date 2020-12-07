namespace CoachEasy.Web.ViewModels.Coaches
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Common;
    using CoachEasy.Data.Models.Enums;
    using Microsoft.AspNetCore.Http;

    public class CreateCourseInputModel
    {
        [Required]
        [Display(Name = "Course name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        [Required(ErrorMessage = GlobalConstants.DescriptionErrorMessage)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "For position")]
        public PositionName PositionName { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Course picture")]
        public IFormFile Image { get; set; }
    }
}
