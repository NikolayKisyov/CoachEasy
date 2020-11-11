namespace CoachEasy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    using CoachEasy.Data.Common.Models;

    public class ContactForm : BaseDeletableModel<string>
    {
        public ContactForm()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Twitter { get; set; }

        [Required]
        public string Instagram { get; set; }
    }
}
