namespace CoachEasy.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class CreateCoachInputModel
    {
        [Required]
        [StringLength(80, ErrorMessage = "Full name must be atleast {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 30)]
        public int Experience { get; set; }

        [StringLength(100, ErrorMessage = "Description must be atleast {2} and at max {1} characters long.", MinimumLength = 30)]
        public string Description { get; set; }

        public string Phone { get; set; }

        public IFormFile UserImage { get; set; }

    }
}
