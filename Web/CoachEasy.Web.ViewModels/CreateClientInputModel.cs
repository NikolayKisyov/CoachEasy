namespace CoachEasy.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using CoachEasy.Data.Models.Enums;

    public class CreateClientInputModel
    {
        [Required]
        [StringLength(80, ErrorMessage = "Full name must be atleast {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool HasExperience { get; set; }

        public PositionName PositionPlayed { get; set; }

        public string Phone { get; set; }
    }
}
